using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.DTOs;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:9000", 
            "http://localhost:5173",
            "https://client-4whjirqps-dhanuashans-projects.vercel.app"
        )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddLogging(l => l.AddConsole());

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

        // Return response
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

app.Run();

// Helper method for password hashing
static string HashPassword(string password)
{
    using var sha256 = SHA256.Create();
    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(bytes);
}

