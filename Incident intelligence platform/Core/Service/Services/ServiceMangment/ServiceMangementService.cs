using Domain.Contracts.Service;
using Incident_intelligence_platform.DTOs.ServiceDTOs;
using Mapster;
using ServiceAbstraction.Contracts.ServiceMangment;


namespace Incident_intelligence_platform.Services
{
    public class ServiceManagementService : IServiceMangementService
    {
        private readonly IServiceRepo _serviceRepo;

        public ServiceManagementService(IServiceRepo serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        public async Task<IEnumerable<GetServiceResponseDTO>> GetAllServicesAsync(int pageNumber, int pageSize)
        {
            var services = await _serviceRepo.GetAllAsync(pageNumber, pageSize);
            return services.Adapt<IEnumerable<GetServiceResponseDTO>>();
        }

        public async Task<GetServiceResponseDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            return service?.Adapt<GetServiceResponseDTO>();
        }

        public async Task<GetServiceResponseDTO> CreateServiceAsync(CreateServiceRequestDTO dto)
        {
            var service = dto.Adapt<ServiceModel>();

            await _serviceRepo.AddAsync(service);
            await _serviceRepo.SaveChangesAsync();

            return service.Adapt<GetServiceResponseDTO>();
        }

        public async Task<GetServiceResponseDTO?> UpdateServiceAsync(int id, UpdateServiceRequestDTO dto)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null) return null;

            dto.Adapt(service);
            _serviceRepo.Update(service);
            await _serviceRepo.SaveChangesAsync();

            return service.Adapt<GetServiceResponseDTO>();
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null) return false;

            _serviceRepo.Delete(service);
            await _serviceRepo.SaveChangesAsync();

            return true;
        }
    }
}