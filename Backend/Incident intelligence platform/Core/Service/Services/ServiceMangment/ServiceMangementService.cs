using Domain.Contracts;
using Domain.Entities.Services;
using Incident_intelligence_platform.DTOs.ServiceDTOs;
using Mapster;
using ServiceAbstraction.Contracts.Caching;
using ServiceAbstraction.Contracts.ServiceMangment;
using ServiceLayer.Services.Specifications;


namespace ServiceLayer.Services.ServiceMangment
{
    public class ServiceManagementService : IServiceMangementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRediesCachingService _cachingService;

        public ServiceManagementService(IUnitOfWork unitOfWork, IRediesCachingService cachingService)
        {
            _unitOfWork = unitOfWork;
            _cachingService = cachingService;
        }

        public async Task<IEnumerable<GetServiceResponseDTO>> GetAllServicesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"services:page:{pageNumber}:size:{pageSize}";

            var cachedData = await _cachingService.GetData<IEnumerable<GetServiceResponseDTO>>(cacheKey, cancellationToken);
            if (cachedData is not null)
            {
                return cachedData;
            }

            var spec = new ServiceSpecification(pageNumber, pageSize);
            var servicesEntity = await _unitOfWork.GetRepository<Service, int>().GetAllAsync(spec);

            if (servicesEntity is null || !servicesEntity.Any())
            {
                return Enumerable.Empty<GetServiceResponseDTO>();
            }

            var servicesDto = servicesEntity.Adapt<IEnumerable<GetServiceResponseDTO>>();

            await _cachingService.SetData(cacheKey, servicesDto, TimeSpan.FromMinutes(10), cancellationToken);

            return servicesDto;
        }

        public async Task<GetServiceResponseDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(id);
            return service?.Adapt<GetServiceResponseDTO>();
        }

        public async Task<GetServiceResponseDTO> CreateServiceAsync(CreateServiceRequestDTO dto)
        {
            var service = dto.Adapt<Service>();

            await _unitOfWork.GetRepository<Service, int>().AddAsync(service);
            await _unitOfWork.SaveChangesAsync();

            return service.Adapt<GetServiceResponseDTO>();
        }

        public async Task<GetServiceResponseDTO?> UpdateServiceAsync(int id, UpdateServiceRequestDTO dto)
        {
            var serviceRepo = _unitOfWork.GetRepository<Service, int>();
            var service = await serviceRepo.GetByIdAsync(id);

            if (service == null) return null;

            dto.Adapt(service);
            serviceRepo.Update(service);
            await _unitOfWork.SaveChangesAsync();

            return service.Adapt<GetServiceResponseDTO>();
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var serviceRepo = _unitOfWork.GetRepository<Service, int>();
            var service = await serviceRepo.GetByIdAsync(id);

            if (service == null) return false;

            serviceRepo.Delete(service);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}