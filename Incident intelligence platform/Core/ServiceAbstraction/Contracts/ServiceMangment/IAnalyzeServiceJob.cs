using Shared.Dtos.ServiceDTOs;

namespace ServiceAbstraction.Contracts.ServiceMangment
{
    public interface IAnalyzeServiceJob
    {
        public Task AnalyzeServices(AnalyzeServiceRequest analyzeServiceDto); // Check Error Nums Per Service , latancy Per Request perServices  
    }
}
