using Domain.Entities.Services;

namespace Domain.Contracts.Services
{
    public interface IServiceRepo
    {
        public Task<IEnumerable<Service>> GetAllAsync(int pageNumber, int pageSize);
        public Task<Service?> GetByIdAsync(int id);
        public Task AddAsync(Service service);
        public void Update(Service service);
        public void Delete(Service service);
        public Task SaveChangesAsync();

    }
}
