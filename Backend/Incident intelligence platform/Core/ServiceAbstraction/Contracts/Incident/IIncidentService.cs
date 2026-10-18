using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Shared;
using Shared.Dtos;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentService
    {
        Task<Result<IEnumerable<GetIncidentResponseDTO>>> GetAllIncidentsAsync(IncidentSpecParams specParams);
        Task<Result<GetIncidentResponseDTO?>> GetIncidentByIdAsync(int id);
        Task<Result<GetIncidentResponseDTO?>> CreateIncidentAsync(CreateIncidentRequestDTO dto);
        Task<Result<GetIncidentResponseDTO?>> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto);
        Task<Result<bool>> DeleteIncidentAsync(int id);
    }
}