using System.Text;
using AirLineManager.dal;
using AirLineManager.dal.Data;
using AirLineManager.dal.Entities;
using AirLineManager.webapi;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Create a new instance of the web application builder
var builder = WebApplication.CreateBuilder(args);

// Register the necessary services
string defaultCors = "defaultCors";
builder.Services.AddCors(options =>
    options.AddPolicy(name: defaultCors, policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    })
);
builder.Services.AddControllers().AddNewtonsoftJson();

// Get the connection string from the appsettings.json file
string? connStr = builder.Configuration.GetConnectionString("Default");

// Add a SQL Server database context to the service collection
builder.Services.AddSqlServer<AirlineDbContext>(connStr);

// Add Identity services for authentication and authorization
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AirlineDbContext>()
    .AddDefaultTokenProviders();

// Add JWT bearer authentication middleware
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration?["JWT:Secret"] ?? ""))
    };
});

// Add necessary singletons and scoped repositories to the service collection
builder.Services.AddSingleton<Mapper>();
builder.Services.AddScoped<Methods>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Swagger and Swagger UI to the service collection
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    // Generate the default UI of Swagger documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 7 Web API",
        Description = "Authentication and Authorization in ASP.NET 7 with JWT and Swagger"
    });

    // Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Build the web application
var app = builder.Build();

// Use Swagger and SwaggerUI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET 7 Web API v1"));
}

// Apply any pending migrations to the database upon scope creation
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<AirlineDbContext>();
    ctx?.Database.Migrate();
}

// Add necessary middleware
app.UseHttpsRedirection();
app.UseCors(defaultCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();