using Domain.Contracts.Services;
using Domain.Entities.Services;
using Incident_intelligence_platform.DTOs.ServiceDTOs;
using Mapster;
using ServiceAbstraction.Contracts.Caching;
using ServiceAbstraction.Contracts.ServiceMangment;

namespace ServiceLayer.Services.ServiceMangment
{
    public class ServiceManagementService : IServiceMangementService
    {
        private readonly IServiceRepo _serviceRepo;
        private readonly IRediesCachingService cachingService;

        public ServiceManagementService(IServiceRepo serviceRepo, IRediesCachingService cachingService)
        {
            _serviceRepo = serviceRepo;
            this.cachingService = cachingService;
        }

        public async Task<IEnumerable<GetServiceResponseDTO>> GetAllServicesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {

            string cacheKey = $"services:page:{pageNumber}:size:{pageSize}";


            var cachedData = await cachingService.GetData<IEnumerable<GetServiceResponseDTO>>(cacheKey, cancellationToken);
            if (cachedData is not null)
            {
                return cachedData;
            }


            var servicesEntity = await _serviceRepo.GetAllAsync(pageNumber, pageSize);
            if (servicesEntity is null)
            {
                return Enumerable.Empty<GetServiceResponseDTO>();
            }


            var servicesDto = servicesEntity.Adapt<IEnumerable<GetServiceResponseDTO>>();


            await cachingService.SetData(cacheKey, servicesDto, TimeSpan.FromMinutes(10), cancellationToken);

            return servicesDto;
        }

        public async Task<GetServiceResponseDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            return service?.Adapt<GetServiceResponseDTO>();
        }

        public async Task<GetServiceResponseDTO> CreateServiceAsync(CreateServiceRequestDTO dto)
        {


            var service = dto.Adapt<Service>();

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