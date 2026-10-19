using Domain.Enums.UserMangement;
using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.Incident;
using Shared;

namespace Presentation.Controllers.Incidents
{
    public class IncidentsController(IIncidentService _incidentService) : ApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllIncidents([FromQuery] IncidentSpecParams specParams)
        {
            var result = await _incidentService.GetAllIncidentsAsync(specParams);

            if (!result.IsSuccess)
            {
                return NotFound(ApiResponse<IEnumerable<GetIncidentResponseDTO>>.FailureResponse(
                    result.ErrorMessage ?? "No incidents found.", statusCode: 404));
            }

            return Ok(ApiResponse<IEnumerable<GetIncidentResponseDTO>>.SuccessResponse(
                result.Data, "Incidents retrieved successfully"));
        }



        [HttpGet("{incidentId:int}")]
        [Authorize]
        public async Task<ActionResult> GetIncident(int incidentId)
        {
            var result = await _incidentService.GetIncidentByIdAsync(incidentId);

            if (!result.IsSuccess)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse(
                    result.ErrorMessage ?? $"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(result.Data));
        }




        [HttpPost]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> CreateIncident([FromBody] CreateIncidentRequestDTO requestDto)
        {
            var result = await _incidentService.CreateIncidentAsync(requestDto);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<GetIncidentResponseDTO>.FailureResponse(
                    result.ErrorMessage ?? "Failed to create incident.", statusCode: 400));
            }

            var apiResponse = ApiResponse<GetIncidentResponseDTO>.SuccessResponse(
                result.Data, "Incident created successfully", statusCode: 201);

            return CreatedAtAction(nameof(GetIncident), new { incidentId = result.Data!.Id }, apiResponse);
        }



        [HttpPut("{incidentId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> UpdateIncident(int incidentId, [FromBody] UpdateIncidentRequestDTO requestDto)
        {
            var result = await _incidentService.UpdateIncidentAsync(incidentId, requestDto);

            if (!result.IsSuccess)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse(
                    result.ErrorMessage ?? $"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(
                result.Data, "Incident updated successfully"));
        }




        [HttpDelete("{incidentId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager}")]
        public async Task<ActionResult> DeleteIncident(int incidentId)
        {
            var result = await _incidentService.DeleteIncidentAsync(incidentId);

            if (!result.IsSuccess)
            {
                return NotFound(ApiResponse<bool>.FailureResponse(
                    result.ErrorMessage ?? $"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Incident deleted successfully"));
        }
    }
}