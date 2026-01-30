using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.DTOs;
using server.Services;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Secure Error: Database Connection String is missing. Ensure DATABASE_URL is set in local environment or .env file.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Allow any origin for development/localtunnel
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddLogging(l => l.AddConsole());
builder.Services.AddSingleton<AIScoringService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enterprise: Global Exception Handling
app.Use(async (context, next) => {
    try {
        await next();
    } catch (Exception ex) {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Institutional API Fault: {Message}", ex.Message);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { 
            success = false, 
            message = "An institutional internal error occurred.",
            error_code = "E-999" 
        });
    }
});

app.UseCors("AllowFrontend");

// Enterprise: Health Check Endpoint
app.MapHealthChecks("/health");

// POST /api/register endpoint
app.MapPost("/api/register", async (RegisterRequest request, AppDbContext db) =>
{
    try
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new RegisterResponse
            {
                Success = false,
                Message = "Email and password are required"
            });
        }

        // Check if user already exists
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return Results.BadRequest(new RegisterResponse
            {
                Success = false,
                Message = "User with this email already exists"
            });
        }

        // Hash password (simple SHA256 for demo - use BCrypt in production)
        var passwordHash = HashPassword(request.Password);

        // Create user
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        // Generate wallet address (simple GUID-based for demo)
        var walletAddress = $"CBDC-{Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper()}";

        // Create wallet
        var wallet = new Wallet
        {
            UserId = user.Id,
            WalletAddress = walletAddress,
            Balance = 0,
            Currency = "CBDC",
            CreatedAt = DateTime.UtcNow
        };

        db.Wallets.Add(wallet);
        await db.SaveChangesAsync();

        // Update response
        return Results.Ok(new RegisterResponse
        {
            Success = true,
            Message = "User registered successfully",
            User = new UserData
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Wallet = new WalletData
                {
                    Id = wallet.Id,
                    WalletAddress = wallet.WalletAddress,
                    Balance = wallet.Balance,
                    Currency = wallet.Currency
                }
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
})
.WithName("Register")
.WithOpenApi();

// POST /api/login endpoint
app.MapPost("/api/login", async (LoginRequest request, AppDbContext db) =>
{
    try
    {
        // 1. Find user by email
        var user = await db.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return Results.Json(new LoginResponse 
            { 
                Success = false, 
                Message = "No account found with this email." 
            }, statusCode: 404);
        }

        // 2. Verify password
        var passwordHash = HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
        {
            return Results.Json(new LoginResponse 
            { 
                Success = false, 
                Message = "Invalid password." 
            }, statusCode: 401);
        }

        // 3. Return user data (matching the register response structure)
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
                Wallet = user.Wallet != null ? new WalletData
                {
                    Id = user.Wallet.Id,
                    WalletAddress = user.Wallet.WalletAddress,
                    Balance = user.Wallet.Balance,
                    Currency = user.Wallet.Currency
                } : null
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

