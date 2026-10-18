using Domain.Entities.Incidents;
using Domain.Enums.UserMangement;
using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.Incident;
using Shared.Dtos.IcidentEventDTOs;

namespace Presentation.Controllers.Incidents
{
    [ApiController]
    [Route("api/incidents")]
    public class IncidentEventController(IIncidentEventService _incidentEventService) : ApiController
    {

        [HttpGet("{id:int}/timeline")]
        [Authorize]
        public async Task<ActionResult> GetTimeLine(int id, [FromQuery] GetTimeLineRequest request)
        {
            var result = await _incidentEventService.GetIncidentTimeLineAsync(id, request.PageSize, request.PageNumber);

            if (!result.IsSuccess)
            {
                return NotFound(ApiResponse<IEnumerable<IncidentEvent>>.FailureResponse(
                    result.ErrorMessage ?? $"Incident with ID {id} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<IEnumerable<GetIncidentEventResponse>>.SuccessResponse(
                result.Data, "Incident timeline retrieved successfully."));
        }



        [HttpPost("{id:int}/events")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> AddEvent(int id, [FromBody] AddIncidentEventDto dto)
        {
            var result = await _incidentEventService.AddEventAsync(id, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<AddIncidentEventResponse>.FailureResponse(
                    result.ErrorMessage ?? "Failed to add incident event.", statusCode: 400));
            }

            var apiResponse = ApiResponse<AddIncidentEventResponse>.SuccessResponse(
                result.Data, "Incident event added successfully.", statusCode: 201);

            return Created($"/api/incidents/{id}/timeline", apiResponse);
        }
    }
}