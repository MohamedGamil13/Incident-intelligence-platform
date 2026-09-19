using Domain.Contracts.Auth;
using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.ServiceDeployments;
using Domain.Contracts.Services;
using Domain.Entities.Users;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Middleware;
using Incident_intelligence_platform.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Persistence.Repositories.Auth;
using Presistance;
using Presistance.Repositories.Auth;
using Presistance.Repositories.Incidents;
using Presistance.Repositories.Logs;
using Presistance.Repositories.ServiceDeployments;
using Presistance.Repositories.Services;
using ServiceAbstraction.Contracts.Auth;
using ServiceAbstraction.Contracts.Incident;
using ServiceAbstraction.Contracts.Logs;
using ServiceAbstraction.Contracts.ServiceDeployments;
using ServiceAbstraction.Contracts.ServiceMangment;
using ServiceLayer.Services.Auth;
using ServiceLayer.Services.Incidents;
using ServiceLayer.Services.Logs;
using ServiceLayer.Services.ServiceDeployments;
using ServiceLayer.Services.ServiceMangment;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

#region Host Configurations
builder.Host.AddSerilogLogging();
#endregion

#region Core Services & Frameworks
builder.Services.AddControllers();

// Custom Validation Response Format
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

// AutoMapper / Mapster Config
builder.Services.RegisterMapsterConfiguration();


// MediatR 
builder.Services.AddMediatR(cfg =>
{

    cfg.RegisterServicesFromAssembly(typeof(LogThresholdHandler).Assembly);


    cfg.RegisterServicesFromAssembly(typeof(LogIngestedEvent).Assembly);
});
#endregion

#region Swagger / API Documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Incident Intelligence Platform API",
        Version = "v1",
        Description = "API Documentation with Role-Based Access Control"
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
        Description = "Enter Token in this format: Bearer {token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
#endregion

#region Infrastructure & Database
builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(AppDbcontext).Assembly.FullName)
    ));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbcontext>()
    .AddDefaultTokenProviders();
#endregion

#region Authentication & Authorization
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

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {

            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = 401,
                error = "Unauthorized",
                message = "You are not authorized to access this resource. Please provide a valid token."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = 403,
                error = "Forbidden",
                message = "You do not have permission to access this resource."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    };
});
#endregion

#region Dependency Injection - Repositories
// Auth Repositories
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IUserMangementRepo, UserMangementRepo>();

// Incident Repositories
builder.Services.AddScoped<IIncidentRepo, IncidentRepository>();
builder.Services.AddScoped<IIncidentEventRepo, IncidentEventRepo>();

// Service Repositories
builder.Services.AddScoped<IServiceRepo, ServiceRepository>();

//Logs Repositories
builder.Services.AddScoped<ILogsRepo, LogsRepo>();

//ServiceDeployment Repositories
builder.Services.AddScoped<IServiceDeploymentsRepo, ServiceDeploymentsRepo>();
#endregion

#region Dependency Injection - Application Services
// Auth Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();

// Incident Services
builder.Services.AddScoped<IIncidentEventService, IncidentEventService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();

// Service Management Services
builder.Services.AddScoped<IServiceMangementService, ServiceManagementService>();

//Logs Services
builder.Services.AddScoped<ILogsService, LogsService>();

//Service Depolyment Services
builder.Services.AddScoped<IServiceDeploymentService, ServiceDeploymentService>();
#endregion

var app = builder.Build();

#region HTTP Request Pipeline & Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HandleTraceIdMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion