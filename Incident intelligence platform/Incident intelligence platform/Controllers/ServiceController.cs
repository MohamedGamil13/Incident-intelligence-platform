using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.DTOs.ServiceDTOs;
using Incident_intelligence_platform.Enums;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ServiceManagementService _serviceService;

        public ServicesController(ServiceManagementService serviceService)
        {
            _serviceService = serviceService;
        }


        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllServices(int pageNumber, int pageSize)
        {
            var services = await _serviceService.GetAllServicesAsync(pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<GetServiceResponseDTO>>.SuccessResponse(services, "Services retrieved successfully"));
        }

        [HttpGet("{serviceId:int}")]
        [Authorize]
        public async Task<IActionResult> GetService(int serviceId)
        {
            var serviceDto = await _serviceService.GetServiceByIdAsync(serviceId);
            if (serviceDto == null)
            {
                return NotFound(ApiResponse<GetServiceResponseDTO>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetServiceResponseDTO>.SuccessResponse(serviceDto));
        }


        [HttpPost]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequestDTO requestDto)
        {
            var responseDto = await _serviceService.CreateServiceAsync(requestDto);
            var apiResponse = ApiResponse<GetServiceResponseDTO>.SuccessResponse(responseDto, "Service created successfully", 201);

            return CreatedAtAction(nameof(GetService), new { serviceId = responseDto.Id }, apiResponse);
        }


        [HttpPut("{serviceId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] UpdateServiceRequestDTO requestDto)
        {
            var updatedService = await _serviceService.UpdateServiceAsync(serviceId, requestDto);
            if (updatedService == null)
            {
                return NotFound(ApiResponse<GetServiceResponseDTO>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetServiceResponseDTO>.SuccessResponse(updatedService, "Service updated successfully"));
        }

        [HttpDelete("{serviceId:int}")]
        [Authorize(Roles = AppUsersRoles.Admin)]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var isDeleted = await _serviceService.DeleteServiceAsync(serviceId);
            if (!isDeleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Service deleted successfully"));
        }
    }
}