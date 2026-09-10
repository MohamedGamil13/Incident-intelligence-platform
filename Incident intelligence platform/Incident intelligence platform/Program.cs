using Incident_intelligence_platform;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Middlewares;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Standard Response 
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {

        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();


        var response = ApiResponse<object>.FailureResponse(
            message: "Validation failed",
            errors: errors,
            statusCode: 400
        );

        return new BadRequestObjectResult(response);
    };
});

builder.Services.RegisterMapsterConfiguration();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//For Auth Override and Configure
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:Key"]!
                )
            )
        };
    });

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbcontext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AuthRepo>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
//Hosts
builder.Host.AddSerilogLogging();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Custom Middlewares
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
