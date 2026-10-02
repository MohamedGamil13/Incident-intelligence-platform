namespace Incident_intelligence_platform.Extensions
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "IncidentPlatform_";
            });

            return services;
        }
    }
}