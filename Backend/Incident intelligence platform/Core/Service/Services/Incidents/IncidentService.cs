using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Entities.Services;
using Domain.Enums.Incident;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Mapster;
using ServiceAbstraction.Contracts.Incident;
using ServiceLayer.Services.Specifications;
using Shared;

namespace ServiceLayer.Services.Incidents
{
    public class IncidentService : IIncidentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncidentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetIncidentResponseDTO>> GetAllIncidentsAsync(IncidentSpecParams specParams)
        {
            var spec = new IncidentWithDetailsSpecification(specParams);
            var incidents = await _unitOfWork.GetRepository<Incident, int>().GetAllAsync(spec);
            return incidents.Adapt<IEnumerable<GetIncidentResponseDTO>>();
        }

        public async Task<GetIncidentResponseDTO?> GetIncidentByIdAsync(int id)
        {
            var spec = new IncidentWithDetailsSpecification(id);
            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(spec);
            return incident?.Adapt<GetIncidentResponseDTO>();
        }

        public async Task<(bool Success, GetIncidentResponseDTO? Data, string ErrorMessage)> CreateIncidentAsync(CreateIncidentRequestDTO dto)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(dto.ServiceId);
            if (service is null)
                return (false, null, $"ServiceId {dto.ServiceId} does not exist.");

            var incident = dto.Adapt<Incident>();
            incident.Status = IncidentStatus.Open;
            incident.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.GetRepository<Incident, int>().AddAsync(incident);
            await _unitOfWork.SaveChangesAsync();

            return (true, incident.Adapt<GetIncidentResponseDTO>(), string.Empty);
        }

        public async Task<GetIncidentResponseDTO?> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto)
        {
            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(id);
            if (incident is null) return null;

            if (!CanTransition(incident.Status, dto.Status))
                throw new InvalidOperationException($"Invalid status transition from {incident.Status} to {dto.Status}.");

            dto.Adapt(incident);

            if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
                incident.ResolvedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return incident.Adapt<GetIncidentResponseDTO>();
        }

        public async Task<bool> DeleteIncidentAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Incident, int>();
            var incident = await repo.GetByIdAsync(id);
            if (incident is null) return false;

            repo.Delete(incident);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static bool CanTransition(IncidentStatus current, IncidentStatus? next)
            => !next.HasValue || current <= next.Value;
    }
}