using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using ServiceAbstraction.Contracts.Incident;
using ServiceLayer.Specifications.IncidentSpecifications;
using Shared.Dtos;
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

        public async Task<Result<IEnumerable<GetIncidentEventResponse>>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber)
        {
            var incidentExists = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(incidentId);
            if (incidentExists is null)
            {
                return Result<IEnumerable<GetIncidentEventResponse>>.Failure($"Incident with ID {incidentId} was not found.");
            }

            var spec = new IncidentEventsByIncidentSpecification(incidentId, pageSize, pageNumber);
            var events = await _unitOfWork.GetRepository<IncidentEvent, int>().GetAllAsync(spec);


            var eventDtos = events.Select(e => new GetIncidentEventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                TimeStamp = e.TimeStamp.ToString(),
                Date = e.Date,
                IncidentId = e.IncidentId
            });

            return Result<IEnumerable<GetIncidentEventResponse>>.Success(eventDtos);
        }

        public async Task<Result<AddIncidentEventResponse>> AddEventAsync(int incidentId, AddIncidentEventDto dto)
        {
            if (dto is null)
            {
                return Result<AddIncidentEventResponse>.Failure("Invalid request payload.");
            }


            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(incidentId);
            if (incident is null)
            {
                return Result<AddIncidentEventResponse>.Failure($"Incident with ID {incidentId} does not exist.");
            }


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


            var response = new AddIncidentEventResponse
            {
                IncidentId = incidentId,
                Description = dto.Description,
                Title = dto.Title,
                IncidentName = incident.Title,
                IcidentStatus = incident.Status,
                IncidentDate = incident.CreatedAt.ToString("o")
            };

            return Result<AddIncidentEventResponse>.Success(response);
        }
    }
}