using System;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechMove.Api.Data;
using TechMove.Api.Services.Interfaces;
using TechMove.Api.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ✅ Connection string - Docker friendly
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? "Server=db;Database=TechMoveDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";

// ✅ DB
// When running integration tests we configure an in-memory provider inside the
// test host. Avoid registering the SQL Server provider when the environment is
// set to 'Testing' so tests can replace the DbContext without provider conflicts.
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(conn));
}

// ✅ Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
builder.Services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddHttpClient();

// ✅ JWT AUTH
var jwtKey = builder.Configuration["JwtKey"] ?? "THIS_IS_A_SUPER_SECRET_KEY_12345";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TechMoveApi",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Detect running in container (Docker) to avoid forcing HTTPS redirection when TLS isn't configured
var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// ✅ Swagger always on
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Static files
app.UseStaticFiles();

// ✅ NO HTTPS - removed for Docker
app.UseAuthentication();
app.UseAuthorization();

// ✅ DB MIGRATION + SEED with retry
if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Migration");

        var maxAttempts = 12;
        var delay = TimeSpan.FromSeconds(5);
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                logger.LogInformation("Attempting database migrate: attempt {Attempt}", attempt);
                db.Database.Migrate();
                SeedData.EnsureSeedData(db);
                logger.LogInformation("Database migration and seed completed.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Database not ready (attempt {Attempt}/{Max}). Retrying in {Delay}s.", attempt, maxAttempts, (int)delay.TotalSeconds);
                if (attempt == maxAttempts)
                {
                    logger.LogError(ex, "Failed to apply migrations after {Max} attempts.", maxAttempts);
                    throw;
                }
                Thread.Sleep(delay);
            }
        }
    }
}

// ✅ Controllers
app.MapControllers();

app.Run();
