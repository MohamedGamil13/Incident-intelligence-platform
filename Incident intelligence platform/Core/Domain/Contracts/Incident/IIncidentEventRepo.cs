using Domain.Entities.Incident;

namespace Domain.Contracts.Incident
{
    public interface IIncidentEventRepo
    {
        public Task<IEnumerable<IncidentEvent>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber);
        public Task AddEventAsync(IncidentEvent newEvent);
        public Task<bool> CheckIncidentExistAsync(int incidentId);

    }
}
