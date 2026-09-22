using Microsoft.Extensions.Caching.Distributed;
using ServiceAbstraction.Contracts.Caching;
using System.Text.Json;

namespace ServiceLayer.Services.Caching
{
    public class RediesCachingService : IRediesCachingService
    {
        private readonly IDistributedCache _cache;

        public RediesCachingService(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<T?> GetData<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(data);

        }

        public async Task SetData<T>(string key, T data)
        {
            var option = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), option);
        }
    }
}
