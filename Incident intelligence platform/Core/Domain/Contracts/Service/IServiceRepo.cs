namespace Domain.Contracts.Service
{
    public interface IServiceRepo
    {
        public Task<IEnumerable<ServiceModel>> GetAllAsync(int pageNumber, int pageSize);
        public Task<ServiceModel?> GetByIdAsync(int id);
        public Task AddAsync(ServiceModel service);
        public void Update(ServiceModel service);
        public void Delete(ServiceModel service);
        public Task SaveChangesAsync();

    }
}
