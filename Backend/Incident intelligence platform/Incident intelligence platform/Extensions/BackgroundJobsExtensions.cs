using Hangfire;
using Incident_intelligence_platform.Config;

namespace Incident_intelligence_platform.Extensions
{
    public static class BackgroundJobsExtensions
    {
        public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));

            services.AddHangfireServer(options => options.WorkerCount = Environment.ProcessorCount * 2);

            return services;
        }

        public static WebApplication UseHangfire(this WebApplication app)
        {
            app.RegisterHangfireJobs();
            app.UseHangfireDashboard("/hangfire");
            return app;
        }
    }
}