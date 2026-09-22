using Microsoft.Extensions.Caching.Distributed;
using ServiceAbstraction.Contracts.Caching;
using System.Text.Json;

namespace ServiceLayer.Services.Caching
{
    public class RediesCachingService : IRediesCachingService
    {
        private readonly IDistributedCache _cache;




        public RediesCachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetData<T>(string key, CancellationToken cancellationToken)
        {
            var data = await _cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }


        public Task<T?> GetData<T>(string key)
        {
            return GetData<T>(key, CancellationToken.None);
        }

        public async Task SetData<T>(string key, T data, TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            };

            var serializedData = JsonSerializer.Serialize(data);

            await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
        }


        public Task SetData<T>(string key, T data)
        {
            return SetData(key, data, TimeSpan.FromMinutes(5), CancellationToken.None);
        }
    }
}