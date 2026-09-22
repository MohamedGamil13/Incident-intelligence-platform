using Incident_intelligence_platform.DTOs.ServiceDTOs;

namespace ServiceAbstraction.Contracts.ServiceMangment
{
    public interface IServiceMangementService
    {
        public Task<IEnumerable<GetServiceResponseDTO>> GetAllServicesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<GetServiceResponseDTO?> GetServiceByIdAsync(int id);
        public Task<GetServiceResponseDTO> CreateServiceAsync(CreateServiceRequestDTO dto);
        public Task<GetServiceResponseDTO?> UpdateServiceAsync(int id, UpdateServiceRequestDTO dto);
        public Task<bool> DeleteServiceAsync(int id);
    }
}
