using Domain.Entities.ServiceDeployments;
using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.ServiceDeployments;
using Shared.Dtos.ServiceDepolyments;

namespace Presentation.Controllers.ServiceDeployments
{
    [ApiController]
    [Route("api/services/{serviceId}/deployments")]
    public class ServiceDeploymentController : ControllerBase
    {
        private readonly IServiceDeploymentService _deploymentService;

        public ServiceDeploymentController(IServiceDeploymentService deploymentService)
        {
            _deploymentService = deploymentService;
        }

        [HttpPost]
        public async Task<ActionResult> RecordDeployment([FromRoute] int serviceId, [FromBody] CreateServiceDeploymentRequest dto)
        {
            var res = await _deploymentService.RecordDeploymentAsync(serviceId, dto);

            if (!res.Success)
            {
                return NotFound(ApiResponse<ServiceDeployment>.FailureResponse(res.errorMessage, null));
            }

            return Ok(ApiResponse<ServiceDeployment>.SuccessResponse(data: res.data, "Deployment Created Successfully"));
        }
    }
}