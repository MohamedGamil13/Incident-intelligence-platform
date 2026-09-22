using Shared.Dtos.ServiceDTOs;

namespace ServiceAbstraction.Contracts.ServiceMangment
{
    public interface IAnalyzeServiceJob
    {
        public Task AnalyzeOneService(AnalyzeServiceRequest analyzeServiceDto);
        public Task AnalyzeAllServices(AnalyzeAllServicesRequest dto);
    }
}
