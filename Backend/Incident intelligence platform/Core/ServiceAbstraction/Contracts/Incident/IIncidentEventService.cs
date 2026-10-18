using Domain.Entities.Incidents;
using Shared.Dtos;
using Shared.Dtos.IcidentEventDTOs;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentEventService
    {
        Task<Result<IEnumerable<IncidentEvent>>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber);
        Task<Result<AddIncidentEventResponse>> AddEventAsync(int incidentId, AddIncidentEventDto dto);
    }
}