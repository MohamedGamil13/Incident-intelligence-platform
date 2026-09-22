namespace ServiceAbstraction.Contracts.Caching
{
    public interface IRediesCachingService
    {
        public Task<T?> GetData<T>(string key);
        public Task SetData<T>(string key, T Data);
    }
}
