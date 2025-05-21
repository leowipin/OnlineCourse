using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineCourse;
using OnlineCourse.Entities;
using OnlineCourse.Middleware;
using OnlineCourse.Services;
using OnlineCourse.Services.IServices;
using OnlineCourse.UnitOfWork;
using Serilog;
using OnlineCourse.Filters;
// ****** BEGIN JWT Added using statements ******
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// ****** END JWT Added using statements ******
#if DEBUG
using SwaggerThemes;
#endif

var builder = WebApplication.CreateBuilder(args);

// Configuraci n de Serilog 
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<SecurityStampValidationFilter>(); // Register the SecurityStampValidationFilter globally
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Course API", Version = "v1" });

    // Ruta del archivo XML de comentarios
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // ****** BEGIN Swagger JWT Configuration ******
    // Define security scheme (JWT Bearer)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    // ****** END Swagger JWT Configuration ******
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAutoMapper((Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IInstructorService, InstructorService>();

// ****** BEGIN JWT AUTHENTICATION AND AUTHSERVICE REGISTRATION ******
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException(
        "JWT Secret ('JwtSettings:Secret') is not configured. " +
        "For Development, check User Secrets (secrets.json) or appsettings.Development.json. " +
        "For Production, ensure it's set via Environment Variables or other secure configuration provider."
    );
}

if (string.IsNullOrEmpty(issuer))
{
    throw new InvalidOperationException(
        "JWT Issuer ('JwtSettings:Issuer') is not configured. " +
        "Check appsettings.json, appsettings.{Environment}.json, User Secrets, or Environment Variables."
    );
}

if (string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException(
        "JWT Audience ('JwtSettings:Audience') is not configured. " +
        "Check appsettings.json, appsettings.{Environment}.json, User Secrets, or Environment Variables."
    );
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true; 
    options.RequireHttpsMetadata = builder.Environment.IsProduction(); 
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)), 
        ClockSkew = TimeSpan.Zero 
    };
});
// ****** END JWT AUTHENTICATION AND AUTHSERVICE REGISTRATION ******

var app = builder.Build();

// Global Exception Middleware
app.UseCustomExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    #if DEBUG
        app.UseSwaggerUI(Theme.UniversalDark);
    #else
        app.UseSwaggerUI();
    #endif
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();