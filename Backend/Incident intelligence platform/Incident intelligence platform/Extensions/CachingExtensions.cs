using StackExchange.Redis;

namespace Incident_intelligence_platform.Extensions
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddStackExchangeRedisCache(options =>
            {
                if (!string.IsNullOrWhiteSpace(redisConnectionString))
                {
                    var configurationOptions = ConfigurationOptions.Parse(redisConnectionString);
                    configurationOptions.AbortOnConnectFail = false;
                    configurationOptions.ConnectTimeout = 2000;
                    configurationOptions.SyncTimeout = 2000;
                    options.ConfigurationOptions = configurationOptions;
                }

                options.InstanceName = "IncidentPlatform_";
            });

            return services;
        }
    }
}
