using Domain.Contracts.Incidents;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Mapster;
using ServiceAbstraction.Contracts.Incident;


namespace ServiceLayer.Services.Incidents
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentRepo _incidentRepo;

        public IncidentService(IIncidentRepo incidentRepo)
        {
            _incidentRepo = incidentRepo;
        }

        public async Task<IEnumerable<GetIncidentResponseDTO>> GetAllIncidentsAsync(int pageNumber, int pageSize)
        {
            var incidents = await _incidentRepo.GetAllAsync(pageNumber, pageSize);
            return incidents.Adapt<IEnumerable<GetIncidentResponseDTO>>();
        }

        public async Task<GetIncidentResponseDTO?> GetIncidentByIdAsync(int id)
        {
            var incident = await _incidentRepo.GetByIdAsync(id);

            var response = incident?.Adapt<GetIncidentResponseDTO>();

            return response;
        }

        public async Task<(bool Success, GetIncidentResponseDTO? Data, string ErrorMessage)> CreateIncidentAsync(CreateIncidentRequestDTO dto)
        {
            var serviceExists = await _incidentRepo.ServiceExistsAsync(dto.ServiceId);
            if (!serviceExists)
            {
                return (false, null, $"ServiceId {dto.ServiceId} does not exist.");
            }

            var incident = dto.Adapt<Incident>();
            incident.Status = IncidentStatus.Open;
            incident.CreatedAt = DateTime.UtcNow;

            await _incidentRepo.AddAsync(incident);
            await _incidentRepo.SaveChangesAsync();

            return (true, incident.Adapt<GetIncidentResponseDTO>(), string.Empty);
        }

        public async Task<GetIncidentResponseDTO?> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto)
        {
            var incident = await _incidentRepo.GetByIdAsync(id);


            if (incident == null) return null;
            if (!CanTransition(dto.Status, incident.Status))
            {
                throw new InvalidOperationException($"Invalid status transition from {incident.Status} to {dto.Status}.");
            }

            dto.Adapt(incident);

            if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
            {
                incident.ResolvedAt = DateTime.UtcNow;
            }
            _incidentRepo.Update(incident);
            await _incidentRepo.SaveChangesAsync();

            return incident.Adapt<GetIncidentResponseDTO>();
        }

        public async Task<bool> DeleteIncidentAsync(int id)
        {
            var incident = await _incidentRepo.GetByIdAsync(id);
            if (incident == null) return false;

            _incidentRepo.Delete(incident);
            await _incidentRepo.SaveChangesAsync();

            return true;
        }
        private bool CanTransition(IncidentStatus? Current, IncidentStatus next)
        {
            if (Current > next)
            {
                return false;
            }
            return true;
        }
    }
}