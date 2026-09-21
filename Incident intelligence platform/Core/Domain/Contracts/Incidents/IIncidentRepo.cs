using Domain.Entities.Incidents;

namespace Domain.Contracts.Incidents
{
    public interface IIncidentRepo
    {
        public Task<IEnumerable<Incident>> GetAllAsync(int pageNumber, int pageSize);
        public Task<Incident?> GetByIdAsync(int id);
        public Task AddAsync(Incident incident);
        public void Update(Incident incident);
        public void Delete(Incident incident);
        public Task<bool> ServiceExistsAsync(int serviceId);
        public Task<IEnumerable<Incident>> GetActiveIncidentPerService(int serviceId, int timeWindowInMin = 5);
        public Task<int> GetActiveIncidentCountPerService(int serviceId, int timeWindowInMin = 5);
        public Task SaveChangesAsync();

    }
}
