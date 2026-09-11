using Incident_intelligence_platform;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Middlewares;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;
using Incident_intelligence_platform.Repositories;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Standard Response Configuration for Validation Failures
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

#region RegisterAPIdoc
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "My API",
        Version = "v1",
        Description = "API Documentation with Role-Based Access"
    });
    options.OperationFilter<SwaggerRoleFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Token In This Way : Bearer {token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

#endregion

builder.Services.RegisterMapsterConfiguration();


// Database Context
builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//Idenetity Config
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbcontext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
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
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)
        )
    };
});

// Register Repositories
builder.Services.AddScoped<AuthRepo>();
builder.Services.AddScoped<IncidentRepository>();
builder.Services.AddScoped<ServiceRepository>();

// Register Services
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IncidentService>();
builder.Services.AddScoped<ServiceManagementService>();

// Logging Host
builder.Host.AddSerilogLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom Middlewares
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
