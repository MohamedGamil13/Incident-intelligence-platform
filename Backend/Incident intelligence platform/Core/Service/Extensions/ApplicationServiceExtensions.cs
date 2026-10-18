using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using ServiceAbstraction.Contracts.Auth;
using ServiceAbstraction.Contracts.Caching;
using ServiceAbstraction.Contracts.Incident;
using ServiceAbstraction.Contracts.Logs;
using ServiceAbstraction.Contracts.ServiceDeployments;
using ServiceAbstraction.Contracts.ServiceMangment;
using ServiceLayer.Services.Auth;
using ServiceLayer.Services.Caching;
using ServiceLayer.Services.Incidents;
using ServiceLayer.Services.Logs;
using ServiceLayer.Services.Logs.Commands;
using ServiceLayer.Services.ServiceDeployments;
using ServiceLayer.Services.ServiceMangment;


namespace ServiceLayer.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LogIngestedEvent).Assembly));

            // Auth
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserMangementService, UserMangmentService>();

            // Incidents
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<IIncidentEventService, IncidentEventService>();

            // Services / deployments / analysis job
            services.AddScoped<IServiceMangementService, ServiceManagementService>();
            services.AddScoped<IServiceDeploymentService, ServiceDeploymentService>();
            services.AddScoped<IAnalyzeServiceJob, AnalyzeServicesJob>();

            // Logs
            services.AddScoped<ILogsService, LogsService>();

            // Caching
            services.AddScoped<IRediesCachingService, RediesCachingService>();
            //Emails
            services.AddOptions();
            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = configuration["Resend:ApiKey"]!;
            });
            services.AddTransient<IResend, ResendClient>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}