using Domain.Contracts.Incidents;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using ServiceAbstraction.Contracts.Incident;
using Shared.Dtos.IcidentEventDTOs;


namespace ServiceLayer.Services.Incidents
{
    public class IncidentEventService : IIncidentEventService
    {
        private readonly IIncidentEventRepo incidentEventRepo;
        private readonly IIncidentRepo incidentRepository;

        public IncidentEventService(IIncidentEventRepo incidentEventRepo, IIncidentRepo incidentRepository)
        {
            this.incidentEventRepo = incidentEventRepo;
            this.incidentRepository = incidentRepository;
        }

        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLine(int incidentId, int pageSize, int pageNumber)
        {
            return await incidentEventRepo.GetIncidentTimeLineAsync(incidentId, pageSize, pageNumber);
        }

        public async Task<(bool Success, AddIncidentEventResponse? Data, string ErrorMessage)> AddEvent(int incidentId, AddIncidentEventDto dto)
        {
            if (dto == null)
            {
                return (false, null, "Invalid Input");
            }

            bool incidentExist =
                await incidentEventRepo.CheckIncidentExistAsync(incidentId);

            if (!incidentExist)
            {
                return (
                    false,
                    null,
                    $"IncidentId {incidentId} does not exist."
                );
            }

            Incident? incident =
                await incidentRepository.GetByIdAsync(incidentId);

            if (incident == null)
            {
                return (
                    false,
                    null,
                    $"IncidentId {incidentId} does not exist."
                );
            }

            IncidentEvent incidentEvent = new IncidentEvent
            {
                Title = dto.Title,
                Description = dto.Description,
                IncidentId = incidentId,
                TimeStamp = dto.TimeStamp ?? IncidentEventTimeStamp.Created,
                Date = DateTime.UtcNow
            };

            await incidentEventRepo.AddEventAsync(incidentEvent);

            return (
                Success: true,

                Data: new AddIncidentEventResponse
                {
                    IncidentId = incidentId,
                    Description = dto.Description,
                    Title = dto.Title,
                    IncidentName = incident.Title,
                    IcidentStatus = incident.Status,
                    IncidentDate = incident.CreatedAt.ToString()
                },

                ErrorMessage: string.Empty
            );
        }

    }
}