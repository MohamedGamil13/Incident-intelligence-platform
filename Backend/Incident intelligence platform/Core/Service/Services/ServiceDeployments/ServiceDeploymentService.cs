using Domain.Contracts;
using Domain.Entities.ServiceDeployments;
using Domain.Entities.Services;
using ServiceAbstraction.Contracts.ServiceDeployments;
using Shared.Dtos.ServiceDepolyments;

namespace ServiceLayer.Services.ServiceDeployments
{
    public class ServiceDeploymentService : IServiceDeploymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceDeploymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, ServiceDeploymentResponseDto? data, string errorMessage)> RecordDeploymentAsync(int serviceId, CreateServiceDeploymentRequest dto)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(serviceId);
            if (service is null)
                return (false, null, "Service not Found");

            var deployment = new ServiceDeployment
            {
                ServiceId = serviceId,
                DeployedAt = DateTime.UtcNow,
                Title = dto.Title,
                Version = dto.Version,
                DeployedBy = dto.DeployedBy
            };

            await _unitOfWork.GetRepository<ServiceDeployment, int>().AddAsync(deployment);
            await _unitOfWork.SaveChangesAsync();

            var response = new ServiceDeploymentResponseDto
            {
                Id = deployment.Id,
                ServiceId = deployment.ServiceId,
                Title = deployment.Title,
                Version = deployment.Version,
                DeployedBy = deployment.DeployedBy,
                DeployedAt = deployment.DeployedAt
            };

            return (true, response, string.Empty);
        }
    }
}