using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ServiceAbstraction.Contracts.Caching;
using System.Text.Json;

namespace ServiceLayer.Services.Caching
{
    public class RediesCachingService : IRediesCachingService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RediesCachingService> _logger;

        public RediesCachingService(IDistributedCache cache, ILogger<RediesCachingService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetData<T>(string key, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _cache.GetStringAsync(key, cancellationToken);

                if (string.IsNullOrEmpty(data))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache read failed for key {CacheKey}. Continuing without cache.", key);
                return default;
            }
        }


        public Task<T?> GetData<T>(string key)
        {
            return GetData<T>(key, CancellationToken.None);
        }

        public async Task SetData<T>(string key, T data, TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = timeSpan
                };

                var serializedData = JsonSerializer.Serialize(data);

                await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache write failed for key {CacheKey}. Continuing without cache.", key);
            }
        }


        public Task SetData<T>(string key, T data)
        {
            return SetData(key, data, TimeSpan.FromMinutes(5), CancellationToken.None);
        }

        public async Task DeleteData(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache delete failed for key {CacheKey}. Continuing without cache.", key);
            }
        }
    }
}
