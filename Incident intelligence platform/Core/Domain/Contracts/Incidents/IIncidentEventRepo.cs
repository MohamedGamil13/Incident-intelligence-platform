using Domain.Entities.Incidents;

namespace Domain.Contracts.Incidents
{
    public interface IIncidentEventRepo
    {
        public Task<IEnumerable<IncidentEvent>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber);
        public Task AddEventAsync(IncidentEvent newEvent);
        public Task<bool> CheckIncidentExistAsync(int incidentId);

    }
}
