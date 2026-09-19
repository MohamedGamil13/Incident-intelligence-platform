using Domain.Contracts.ServiceDeployments;
using Domain.Contracts.Services;
using Domain.Entities.ServiceDeployments;
using ServiceAbstraction.Contracts.ServiceDeployments;
using Shared.Dtos.ServiceDepolyments;

namespace ServiceLayer.Services.ServiceDeployments
{
    public class ServiceDeploymentService : IServiceDeploymentService
    {
        private readonly IServiceDeploymentsRepo deploymentsRepo;
        private readonly IServiceRepo serviceRepo;

        public ServiceDeploymentService(IServiceDeploymentsRepo deploymentsRepo, IServiceRepo serviceRepo)
        {
            this.deploymentsRepo = deploymentsRepo;
            this.serviceRepo = serviceRepo;
        }
        public async Task<(bool Success, ServiceDeployment? data, string errorMessage)> RecordDeploymentAsync(int serviceId, CreateServiceDeploymentRequest dto)
        {
            var service = await serviceRepo.GetByIdAsync(serviceId);
            if (service == null)
            {
                return (false, null, "Service not Found");
            }

            ServiceDeployment newDep = new ServiceDeployment()
            {
                ServiceId = serviceId,
                Service = service,
                DeployedAt = DateTime.UtcNow,
                Title = dto.Title,
                Version = dto.Version,
                DeployedBy = dto.DeployedBy,
            };
            await deploymentsRepo.AddAsync(newDep);
            await deploymentsRepo.SaveChangesAsync();

            return (true, newDep, string.Empty);
        }
    }
}