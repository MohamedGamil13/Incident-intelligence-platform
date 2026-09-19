using Domain.Enums.UserMangement;
using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = $"{AppUsersRoles.IncidentManager}, {AppUsersRoles.Admin}")]
        public async Task<ActionResult> RecordDeployment([FromRoute] int serviceId, [FromBody] CreateServiceDeploymentRequest dto)
        {
            var res = await _deploymentService.RecordDeploymentAsync(serviceId, dto);

            if (!res.Success)
            {
                return NotFound(ApiResponse<ServiceDeploymentResponseDto>.FailureResponse(res.errorMessage, null));
            }

            return Ok(ApiResponse<ServiceDeploymentResponseDto>.SuccessResponse(data: res.data, "Deployment Created Successfully"));
        }
    }
}