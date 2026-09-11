using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Incident_intelligence_platform.Enums;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly IncidentService _incidentService;

        public IncidentController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }



        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult> GetAllIncidents(int pageNumber, int pageSize)
        {
            var incidents = await _incidentService.GetAllIncidentsAsync(pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<GetIncidentResponseDTO>>.SuccessResponse(incidents, "Incidents retrieved successfully"));
        }



        [HttpGet("{incidentId:int}")]
        public async Task<ActionResult> GetIncident(int incidentId)
        {
            var incidentDto = await _incidentService.GetIncidentByIdAsync(incidentId);
            if (incidentDto == null)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(incidentDto));
        }



        [HttpPost]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<ActionResult> CreateIncident([FromBody] CreateIncidentRequestDTO requestDto)
        {
            var result = await _incidentService.CreateIncidentAsync(requestDto);
            if (!result.Success)
            {
                return BadRequest(ApiResponse<GetIncidentResponseDTO>.FailureResponse(result.ErrorMessage));
            }

            var apiResponse = ApiResponse<GetIncidentResponseDTO>.SuccessResponse(result.Data, "Incident created successfully", 201);
            return CreatedAtAction(nameof(GetIncident), new { incidentId = result.Data!.Id }, apiResponse);
        }




        [HttpPut("{incidentId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<ActionResult> UpdateIncident(int incidentId, [FromBody] UpdateIncidentRequestDTO requestDto)
        {
            var updatedIncident = await _incidentService.UpdateIncidentAsync(incidentId, requestDto);
            if (updatedIncident == null)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(updatedIncident, "Incident updated successfully"));
        }




        [HttpDelete("{incidentId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<ActionResult> DeleteIncident(int incidentId)
        {
            var isDeleted = await _incidentService.DeleteIncidentAsync(incidentId);
            if (!isDeleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Incident deleted successfully"));
        }
    }
}