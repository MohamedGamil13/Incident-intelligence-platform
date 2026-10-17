using Domain.Entities.ServiceDeployments;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Specifications.ServiceSpecifications
{
    public class DeploymentsByServiceSpecification : BaseSpecification<ServiceDeployment, int>
    {
        public DeploymentsByServiceSpecification(int serviceId)
            : base(d => d.ServiceId == serviceId)
        {
            AddOrderByDescending(d => d.DeployedAt);
        }
    }
}

// All deployments, newest first:
// await repo.GetAllAsync(new DeploymentsByServiceSpecification(id));
// Latest deployment only:
// await repo.GetByIdAsync(new DeploymentsByServiceSpecification(id));