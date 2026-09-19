using Domain.Entities.ServiceDeployments;

namespace Domain.Contracts.ServiceDeployments
{
    public interface IServiceDeploymentsRepo
    {
        public Task AddAsync(ServiceDeployment deployment);

        public Task<ServiceDeployment?> GetLatestDeploymentByServiceIdAsync(int serviceId);

        public Task<IEnumerable<ServiceDeployment>> GetAllDeploymentByService(int serviceId);

        public Task SaveChangesAsync();
    }
}