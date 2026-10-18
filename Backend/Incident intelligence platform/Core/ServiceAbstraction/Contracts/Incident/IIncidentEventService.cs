using Shared.Dtos;
using Shared.Dtos.IcidentEventDTOs;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentEventService
    {
        Task<Result<IEnumerable<GetIncidentEventResponse>>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber);
        Task<Result<AddIncidentEventResponse>> AddEventAsync(int incidentId, AddIncidentEventDto dto);
    }
}