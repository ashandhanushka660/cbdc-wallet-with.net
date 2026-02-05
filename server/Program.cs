using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.DTOs;
using server.Services;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Secure Error: Database Connection String is missing. Ensure DATABASE_URL is set in local environment or .env file.");
}

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(connectionString + ";Max Auto Prepare=0;", npgsqlOptions => {
//         npgsqlOptions.EnableRetryOnFailure(
//             maxRetryCount: 10,
//             maxRetryDelay: TimeSpan.FromSeconds(30),
//             errorCodesToAdd: null);
//     }));

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
        // Note: AllowAnyOrigin and AllowCredentials cannot be used together.
        // For development, AllowAnyOrigin is safer.
    });
});

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddLogging(l => l.AddConsole());

// AI Architecture Services
builder.Services.AddSingleton<AIScoringService>();
builder.Services.AddHttpClient<AIPredictionService>();
builder.Services.AddHostedService<CreditScoringWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// Enterprise: Global Exception Handling
app.Use(async (context, next) => {
    try {
        await next();
    } catch (Exception ex) {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Institutional API Fault: {Message}", ex.Message);
        
        // Ensure we always return JSON even on system errors
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { 
            success = false, 
            message = ex.Message,
            innerError = ex.InnerException?.Message,
            errorCode = "E-999" 
        });
    }
});

// Enterprise: Health Check Endpoint
app.MapHealthChecks("/health");

// POST /api/register endpoint
app.MapPost("/api/register", async (RegisterRequest request) =>
{
    try
    {
        // 1. Validate
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return Results.BadRequest(new RegisterResponse { Success = false, Message = "Email and password are required" });

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("apikey", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");

        // 2. Check duplicate and Create via REST
        client.DefaultRequestHeaders.Add("Prefer", "return=representation");
        var checkRes = await client.GetAsync($"https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Users?Email=eq.{request.Email}");
        var existing = await checkRes.Content.ReadFromJsonAsync<List<User>>();
        if (existing != null && existing.Any())
            return Results.BadRequest(new RegisterResponse { Success = false, Message = "User already exists" });

        // Hash password
        var passwordHash = HashPassword(request.Password);

        // Create user in Supabase via REST (Force PascalCase to match DB)
        var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = null };
        var userRecord = new
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var userResponse = await client.PostAsJsonAsync("https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Users", userRecord, options);
        if (!userResponse.IsSuccessStatusCode)
        {
            var error = await userResponse.Content.ReadAsStringAsync();
            return Results.Json(new { success = false, message = $"Identity Fault: {error}" }, statusCode: 400);
        }

        var users = await userResponse.Content.ReadFromJsonAsync<List<UserData>>(options);
        var newUser = users?.FirstOrDefault();

        if (newUser == null) throw new Exception("Failed to retrieve created user identity.");

        // Generate wallet address
        var walletAddress = $"CBDC-{Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper()}";

        // Create wallet in Supabase via REST
        var walletRecord = new
        {
            UserId = newUser.Id,
            WalletAddress = walletAddress,
            Balance = 0,
            Currency = "CBDC",
            CreatedAt = DateTime.UtcNow
        };

        var walletResponse = await client.PostAsJsonAsync("https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Wallets", walletRecord, options);
        if (!walletResponse.IsSuccessStatusCode)
        {
            var error = await walletResponse.Content.ReadAsStringAsync();
            return Results.Json(new { success = false, message = $"Wallet Genesis Fault: {error}" }, statusCode: 400);
        }

        var wallets = await walletResponse.Content.ReadFromJsonAsync<List<WalletData>>(options);
        var newWallet = wallets?.FirstOrDefault();

        Console.WriteLine($"[REGISTER REST] Success for {request.Email}");

        return Results.Ok(new RegisterResponse
        {
            Success = true,
            Message = "User registered successfully",
            User = new UserData
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Wallet = newWallet
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[REGISTER ERROR] {ex}");
        return Results.Json(new { 
            success = false, 
            message = $"Database/Server Error: {ex.Message}" 
        }, statusCode: 500);
    }
})
.WithName("Register")
.WithOpenApi();

// POST /api/login endpoint
app.MapPost("/api/login", async (LoginRequest request) =>
{
    try
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("apikey", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");

        // 1. Find user by email
        var userResponse = await client.GetAsync($"https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Users?Email=eq.{request.Email}");
        if (!userResponse.IsSuccessStatusCode) return Results.StatusCode(500);

        var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = null };
        var users = await userResponse.Content.ReadFromJsonAsync<List<User>>(options);
        var user = users?.FirstOrDefault();

        if (user == null)
        {
            return Results.Json(new { success = false, message = "No account found with this email." }, statusCode: 404);
        }

        // 2. Verify password
        var passwordHash = HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
        {
            return Results.Json(new { success = false, message = "Invalid password." }, statusCode: 401);
        }

        // 3. Get Wallet
        var walletResponse = await client.GetAsync($"https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Wallets?UserId=eq.{user.Id}");
        var wallets = await walletResponse.Content.ReadFromJsonAsync<List<WalletData>>(options);
        var wallet = wallets?.FirstOrDefault();

        return Results.Ok(new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            User = new UserData
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreditScore = user.CreditScore,
                Wallet = wallet
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
})
.WithName("Login")
.WithOpenApi();

app.MapGet("/api/user/{id}", async (int id) =>
{
    try 
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("apikey", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi");

        var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = null };
        var userResponse = await client.GetAsync($"https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Users?Id=eq.{id}");
        var users = await userResponse.Content.ReadFromJsonAsync<List<User>>(options);
        var user = users?.FirstOrDefault();

        if (user == null) return Results.NotFound();

        var walletResponse = await client.GetAsync($"https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1/Wallets?UserId=eq.{user.Id}");
        var wallets = await walletResponse.Content.ReadFromJsonAsync<List<WalletData>>(options);
        var wallet = wallets?.FirstOrDefault();

        return Results.Ok(new UserData
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreditScore = user.CreditScore,
            Wallet = wallet
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetUser")
.WithOpenApi();

// AI Credit Scoring Endpoint
app.MapGet("/api/ai/score", (AIScoringService aiService, double? telco, double? utility, double? wallet, double? social) =>
{
    // Default mock values if not provided
    double t = telco ?? 0.85;
    double u = utility ?? 0.70;
    double w = wallet ?? 0.90;
    double s = social ?? 0.60;

    double score = aiService.CalculateScore(t, u, w, s);
    
    // Calculate recovery index for a mock 30-day window
    double recoveryIndex = aiService.CalculateRecoveryIndex(30, (t + u) * 10);

    return Results.Ok(new
    {
        success = true,
        score = score,
        recovery_index = recoveryIndex,
        breakdown = new
        {
            telco_contribution = t,
            utility_contribution = u,
            wallet_velocity = w,
            social_reputation = s
        },
        algorithm = "Logistic-GBM Ensemble",
        region = "Sri Lanka (CRIB Aligned)",
        compliant = true
    });
})
.WithName("GetAIScore")
.WithOpenApi();

app.Run();

// Helper method for password hashing
static string HashPassword(string password)
{
    using var sha256 = SHA256.Create();
    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(bytes);
}

