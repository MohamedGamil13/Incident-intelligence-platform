using Incident_intelligence_platform.DTOs.IcidentEventDTOs;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;
using Incident_intelligence_platform.Repositories;

namespace Incident_intelligence_platform.Services
{
    public class IncidentEventService
    {
        private readonly IncidentEventRepo incidentEventRepo;
        private readonly IncidentRepository incidentRepository;

        public IncidentEventService(
            IncidentEventRepo incidentEventRepo,
            IncidentRepository incidentRepository)
        {
            this.incidentEventRepo = incidentEventRepo;
            this.incidentRepository = incidentRepository;
        }

        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLine(
            int incidentId,
            int pageSize,
            int pageNumber)
        {
            return await incidentEventRepo.GetIncidentTimeLineAsync(
                incidentId,
                pageSize,
                pageNumber
            );
        }

        public async Task<(
            bool Success,
            AddIncidentEventResponse? Data,
            string ErrorMessage
        )> AddEvent(
            int incidentId,
            AddIncidentEventDto dto)
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
                TimeStamp = dto.TimeStamp,
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