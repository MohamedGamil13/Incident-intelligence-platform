using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Entities.Services;
using Domain.Enums.Incident;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Mapster;
using ServiceAbstraction.Contracts.Incident;
using ServiceLayer.Specifications.IncidentSpecifications;
using Shared;
using Shared.Dtos;

namespace ServiceLayer.Services.Incidents
{
    public class IncidentService : IIncidentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncidentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<GetIncidentResponseDTO>>> GetAllIncidentsAsync(IncidentSpecParams specParams)
        {
            var spec = new IncidentWithDetailsSpecification(specParams);
            var incidents = await _unitOfWork.GetRepository<Incident, int>().GetAllAsync(spec);

            if (incidents == null || !incidents.Any())
            {
                return Result<IEnumerable<GetIncidentResponseDTO>>.Failure("No incidents found matching the specified criteria.");
            }

            var mappedIncidents = incidents.Adapt<IEnumerable<GetIncidentResponseDTO>>();
            return Result<IEnumerable<GetIncidentResponseDTO>>.Success(mappedIncidents);
        }

        public async Task<Result<GetIncidentResponseDTO?>> GetIncidentByIdAsync(int id)
        {
            var spec = new IncidentWithDetailsSpecification(id);
            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(spec);

            if (incident is null)
            {
                return Result<GetIncidentResponseDTO?>.Failure($"Incident with ID {id} was not found.");
            }

            var mappedIncident = incident.Adapt<GetIncidentResponseDTO>();
            return Result<GetIncidentResponseDTO?>.Success(mappedIncident);
        }

        public async Task<Result<GetIncidentResponseDTO?>> CreateIncidentAsync(CreateIncidentRequestDTO dto)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(dto.ServiceId);
            if (service is null)
            {
                return Result<GetIncidentResponseDTO?>.Failure($"Service with ID {dto.ServiceId} does not exist.");
            }

            var incident = dto.Adapt<Incident>();
            incident.Status = IncidentStatus.Open;
            incident.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.GetRepository<Incident, int>().AddAsync(incident);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = incident.Adapt<GetIncidentResponseDTO>();
            return Result<GetIncidentResponseDTO?>.Success(responseDto);
        }

        public async Task<Result<GetIncidentResponseDTO?>> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto)
        {
            var incident = await _unitOfWork.GetRepository<Incident, int>().GetByIdAsync(id);
            if (incident is null)
            {
                return Result<GetIncidentResponseDTO?>.Failure($"Incident with ID {id} was not found.");
            }

            if (!CanTransition(incident.Status, dto.Status))
            {
                return Result<GetIncidentResponseDTO?>.Failure($"Invalid status transition from {incident.Status} to {dto.Status}.");
            }

            dto.Adapt(incident);

            if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
            {
                incident.ResolvedAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();

            var updatedDto = incident.Adapt<GetIncidentResponseDTO>();
            return Result<GetIncidentResponseDTO?>.Success(updatedDto);
        }

        public async Task<Result<bool>> DeleteIncidentAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Incident, int>();
            var incident = await repo.GetByIdAsync(id);
            if (incident is null)
            {
                return Result<bool>.Failure($"Incident with ID {id} was not found.");
            }

            repo.Delete(incident);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        private static bool CanTransition(IncidentStatus current, IncidentStatus? next)
            => !next.HasValue || current <= next.Value;
    }
}