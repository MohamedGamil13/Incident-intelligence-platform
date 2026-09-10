using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repositories;
using Mapster;

namespace Incident_intelligence_platform.Services
{
    public class IncidentService
    {
        private readonly IncidentRepository _incidentRepo;

        public IncidentService(IncidentRepository incidentRepo)
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
            return incident?.Adapt<GetIncidentResponseDTO>();
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
    }
}