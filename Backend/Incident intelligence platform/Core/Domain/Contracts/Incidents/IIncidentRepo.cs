using Domain.Entities.Incidents;

namespace Domain.Contracts.Incidents
{
    public interface IIncidentRepo : IGenericRepo<Incident, int>
    {
        Task<bool> ServiceExistsAsync(int serviceId);
        Task<IEnumerable<Incident>> GetActiveIncidentPerService(int serviceId, int timeWindowInMin = 5);
        Task<int> GetActiveIncidentCountPerService(int serviceId, int timeWindowInMin = 5);
    }
}