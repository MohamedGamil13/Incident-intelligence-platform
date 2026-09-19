using Shared.Dtos.ServiceDepolyments;

namespace ServiceAbstraction.Contracts.ServiceDeployments
{
    public interface IServiceDeploymentService
    {
        public Task<(bool Success, ServiceDeploymentResponseDto? data, string errorMessage)> RecordDeploymentAsync(int serviceId, CreateServiceDeploymentRequest dto);
    }
}
