using Domain.Entities.Incidents;

namespace Domain.Contracts.Incidents
{
    public interface IIncidentRepo : IGenericRepo<Incident, int>
    {
        Task<bool> ServiceExistsAsync(int serviceId);
    }
}