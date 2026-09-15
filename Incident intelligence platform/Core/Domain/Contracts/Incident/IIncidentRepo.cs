using Domain.Entities.Incident;

namespace Domain.Contracts.Incident
{
    public interface IIncidentRepo
    {
        public Task<IEnumerable<IncidentModel>> GetAllAsync(int pageNumber, int pageSize);
        public Task<IncidentModel?> GetByIdAsync(int id);
        public Task AddAsync(IncidentModel incident);
        public void Update(IncidentModel incident);
        public void Delete(IncidentModel incident);
        public Task<bool> ServiceExistsAsync(int serviceId);
        public Task SaveChangesAsync();

    }
}
