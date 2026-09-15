using Incident_intelligence_platform.DTOs.IncidentDTOs;

namespace ServiceAbstraction.Contracts.Incident
{
    public interface IIncidentService
    {
        public Task<IEnumerable<GetIncidentResponseDTO>> GetAllIncidentsAsync(int pageNumber, int pageSize);
        public Task<GetIncidentResponseDTO?> GetIncidentByIdAsync(int id);
        public Task<(bool Success, GetIncidentResponseDTO? Data, string ErrorMessage)> CreateIncidentAsync(CreateIncidentRequestDTO dto);
        public Task<GetIncidentResponseDTO?> UpdateIncidentAsync(int id, UpdateIncidentRequestDTO dto);
        public Task<bool> DeleteIncidentAsync(int id);

    }
}
