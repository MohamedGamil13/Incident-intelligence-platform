using Domain.Entities.ServiceDeployments;
using Shared.Dtos.ServiceDepolyments;

namespace ServiceAbstraction.Contracts.ServiceDeployments
{
    public interface IServiceDeploymentService
    {
        public Task<(bool Success, ServiceDeployment? data, string errorMessage)> RecordDeploymentAsync(int serviceId, CreateServiceDeploymentRequest dto);
    }
}
