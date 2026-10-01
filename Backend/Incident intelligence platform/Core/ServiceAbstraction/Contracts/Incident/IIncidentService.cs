using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Shared;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentService
    {
        Task<IEnumerable<GetIncidentResponseDTO>> GetAllIncidentsAsync(IncidentSpecParams specParams);
        Task<GetIncidentResponseDTO?> GetIncidentByIdAsync(int id);
        Task<(bool Success, GetIncidentResponseDTO? Data, string ErrorMessage)> CreateIncidentAsync(CreateIncidentRequestDTO dto);
        Task<GetIncidentResponseDTO?> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto);
        Task<bool> DeleteIncidentAsync(int id);
    }
}
