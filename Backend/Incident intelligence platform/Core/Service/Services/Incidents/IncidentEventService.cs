using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using ServiceAbstraction.Contracts.Incident;
using ServiceLayer.Services.Specifications;
using Shared.Dtos.IcidentEventDTOs;

namespace ServiceLayer.Services.Incidents
{
    public class IncidentEventService : IIncidentEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncidentEventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLine(int incidentId, int pageSize, int pageNumber)
        {
            var spec = new IncidentEventsByIncidentSpecification(incidentId, pageSize, pageNumber);
            return await _unitOfWork.GetRepository<IncidentEvent, int>().GetAllAsync(spec);
        }

        public async Task<(bool Success, AddIncidentEventResponse? Data, string ErrorMessage)> AddEvent(int incidentId, AddIncidentEventDto dto)
        {
            if (dto is null)
                return (false, null, "Invalid Input");

            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(incidentId);
            if (incident is null)
                return (false, null, $"IncidentId {incidentId} does not exist.");

            var incidentEvent = new IncidentEvent
            {
                Title = dto.Title,
                Description = dto.Description,
                IncidentId = incidentId,
                TimeStamp = dto.TimeStamp ?? IncidentEventTimeStamp.Created,
                Date = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<IncidentEvent, int>().AddAsync(incidentEvent);
            await _unitOfWork.SaveChangesAsync();

            return (true, new AddIncidentEventResponse
            {
                IncidentId = incidentId,
                Description = dto.Description,
                Title = dto.Title,
                IncidentName = incident.Title,
                IcidentStatus = incident.Status,
                IncidentDate = incident.CreatedAt.ToString()
            }, string.Empty);
        }
    }
}