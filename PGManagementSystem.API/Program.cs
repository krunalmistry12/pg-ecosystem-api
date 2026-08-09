using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Application.Services;
using PGManagementSystem.Infrastructure.Data;
using PGManagementSystem.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using PGManagementSystem.Infrastructure.Services;

var options = new WebApplicationOptions
{
    Args = args,
    // Agar environment variable set nahi hai toh production default le lega
};
var builder = WebApplication.CreateBuilder(options);

// Linux/Render container par inotify limit error ko rokne ke liye:
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);
// 1. Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Add Controllers & JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

// 3. Swagger with Proper Bearer Header Support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PG Management System API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste ONLY your JWT Token below (Do NOT type 'Bearer ' manually):"
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
            Array.Empty<string>()
        }
    });
});

// 4. MySQL Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41))
    ));

// Host URLs
//builder.WebHost.UseUrls(
//    "http://0.0.0.0:5264",
//    "https://0.0.0.0:7180"
//);

// 5. Dependency Injection Registrations (Repositories & Services)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleRepository>();
builder.Services.AddScoped<IPGService, PGRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PGService>();

// Tenant Registrations
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITenantService, TenantService>();

// Bed Registrations
builder.Services.AddScoped<IBedRepository, BedRepository>();

// Flat Registrations
builder.Services.AddScoped<IFlatRepository, FlatRepository>();
builder.Services.AddScoped<IFlatService, FlatService>();

// Dashboard Registrations
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// ?? Rent Module Registrations (MISSING THA - AB ADD KAR DIYA HAI)
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<IRentService, RentService>();

builder.Services.AddHttpClient<IOtpService, OtpService>();

// 6. JWT Authentication Configuration
var jwtSecret = builder.Configuration["Jwt:Key"] ?? "THIS_IS_MY_VERY_SECURE_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_123456";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = false,   // Set to false to prevent 401 due to domain mismatch in Dev
        ValidateAudience = false, // Set to false to prevent 401 due to domain mismatch in Dev
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Expired tokens instantly reject ho jayein
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// 7. Middleware Pipeline Configuration
app.UseCors("AllowAll");

app.UseStaticFiles();

// Swagger ko Production (Render) par bhi enable karne ke liye:
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PG Management System API V1");
    c.RoutePrefix = string.Empty; // Yeh line lagane se direct base URL par hi Swagger khul jayega!
});

app.UseAuthentication(); // ?? Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();