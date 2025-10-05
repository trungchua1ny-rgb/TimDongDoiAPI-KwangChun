using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using TimDongDoi.API.Data;
using TimDongDoi.API.Services.Interfaces;
using TimDongDoi.API.Services.Implementations;
using TimDongDoi.API.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// HttpContextAccessor for accessing HTTP context
builder.Services.AddHttpContextAccessor();

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JwtSettings:SecretKey is missing or empty in configuration.");
}

Console.WriteLine($"[CONFIG] JWT Issuer: {issuer}");
Console.WriteLine($"[CONFIG] JWT Audience: {audience}");
Console.WriteLine($"[CONFIG] JWT SecretKey Length: {secretKey.Length}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"\n[JWT] ==== Message Received ====");
                Console.WriteLine($"[JWT] Path: {context.Request.Path}");
                Console.WriteLine($"[JWT] Auth Header: {(string.IsNullOrEmpty(authHeader) ? "EMPTY" : authHeader.Substring(0, Math.Min(60, authHeader.Length)) + "...")}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"[JWT] ✅ TOKEN VALIDATED SUCCESSFULLY!");
                var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                Console.WriteLine($"[JWT] User ID: {userId}");
                Console.WriteLine($"[JWT] Email: {email}");
                Console.WriteLine($"[JWT] ==========================\n");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"[JWT] ❌ AUTHENTICATION FAILED!");
                Console.WriteLine($"[JWT] Exception Type: {context.Exception.GetType().Name}");
                Console.WriteLine($"[JWT] Exception Message: {context.Exception.Message}");
                if (context.Exception.InnerException != null)
                {
                    Console.WriteLine($"[JWT] Inner Exception: {context.Exception.InnerException.Message}");
                }
                Console.WriteLine($"[JWT] ==========================\n");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"[JWT] ⚠️ CHALLENGE TRIGGERED!");
                Console.WriteLine($"[JWT] Error: {context.Error}");
                Console.WriteLine($"[JWT] Error Description: {context.ErrorDescription}");
                Console.WriteLine($"[JWT] ==========================\n");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger with JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT Token (chỉ token, không cần thêm 'Bearer')"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// IMPORTANT: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Test endpoint
app.MapGet("/", () => new
{
    Message = "Welcome to Tìm Đồng Đội API",
    Version = "1.0.0",
    Status = "Running",
    Timestamp = DateTime.UtcNow
});

Console.WriteLine("\n🚀 Application started successfully!");
Console.WriteLine($"📍 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"🔐 JWT Authentication: Enabled\n");

app.Run();