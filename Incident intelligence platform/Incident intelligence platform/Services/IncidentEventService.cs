using Incident_intelligence_platform.DTOs.IcidentEventDTOs;
using Incident_intelligence_platform.Enums;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;
using Incident_intelligence_platform.Repositories;

namespace Incident_intelligence_platform.Services
{
    public class IncidentEventService
    {
        private readonly IncidentEventRepo incidentEventRepo;
        private readonly IncidentRepository incidentRepository;

        public IncidentEventService(IncidentEventRepo incidentEventRepo, IncidentRepository incidentRepository)
        {
            this.incidentEventRepo = incidentEventRepo;
            this.incidentRepository = incidentRepository;
        }

        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLine(GetTimeLineRequest dto)
        {
            return await incidentEventRepo.GetIncidentTimeLineAsync(dto.IncidentId, dto.PageSize, dto.PageNumber);
        }

        public async Task<(bool Success, AddIncidentEventResponse? Data, string ErrorMessage)> AddEvent(AddIncidentEventDto dto)
        {
            if (dto == null)
            {
                return (false, null, "Invalid Input");
            }
            bool incidentExist = await incidentEventRepo.CheckIncidentExist(dto.IncidentId);
            if (!incidentExist)
            {
                return (true, null, $"ServiceId {dto.IncidentId} does not exist.");
            }
            Incident? incident = await incidentRepository.GetByIdAsync(dto.IncidentId);

            IncidentEvent incidentEvent = new IncidentEvent()
            {
                Title = dto.Title,
                Description = dto.Description,
                IncidentId = dto.IncidentId,
                TimeStamp = IncidentEventTimeStamp.Created,
                Date = DateTime.Now,
            };

            return (Success: true, Data: new AddIncidentEventResponse
            {
                IncidentId = dto.IncidentId,
                Description = dto.Description,
                Title = dto.Title,
                IncidentName = incident.Title,
                IcidentStatus = incident.Status,
                IncidentDate = incident.CreatedAt.ToString(),
            },
            ErrorMessage: string.Empty
            );
        }



    }
}
