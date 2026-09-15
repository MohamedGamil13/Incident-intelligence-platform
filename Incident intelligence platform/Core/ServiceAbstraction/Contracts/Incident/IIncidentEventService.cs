using Domain.Entities.Incident;
using Shared.Dtos.IcidentEventDTOs;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentEventService
    {
        public Task<IEnumerable<IncidentEvent>> GetIncidentTimeLine(int incidentId, int pageSize, int pageNumber);
        public Task<(bool Success, AddIncidentEventResponse? Data, string ErrorMessage)> AddEvent(int incidentId, AddIncidentEventDto dto);


    }
}
