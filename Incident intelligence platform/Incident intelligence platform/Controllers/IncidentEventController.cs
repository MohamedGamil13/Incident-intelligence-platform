using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.DTOs.IcidentEventDTOs;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Incident_intelligence_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentEventController : ControllerBase
    {
        private readonly IncidentEventService incidentEventService;

        public IncidentEventController(
            IncidentEventService incidentEventService)
        {
            this.incidentEventService = incidentEventService;
        }



        [HttpGet("/api/incidents/{id:int}/timeline")]
        public async Task<ActionResult> TimeLine(
            [FromRoute] int id,
            [FromQuery] GetTimeLineRequest request)
        {
            var result =
                await incidentEventService.GetIncidentTimeLine(
                    id,
                    request.PageSize,
                    request.PageNumber
                );

            return Ok(
                ApiResponse<IEnumerable<IncidentEvent>>
                    .SuccessResponse(data: result)
            );
        }



        [HttpPost("/api/incidents/{id:int}/events")]
        public async Task<ActionResult> AddEvent(
            [FromRoute] int id,
            [FromBody] AddIncidentEventDto dto)
        {
            var result =
                await incidentEventService.AddEvent(id, dto);

            if (!result.Success)
            {
                return BadRequest(
                    ApiResponse<AddIncidentEventResponse>
                        .FailureResponse(
                            message: result.ErrorMessage
                        )
                );
            }

            return Ok(
                ApiResponse<AddIncidentEventResponse>
                    .SuccessResponse(
                        data: result.Data
                    )
            );
        }
    }
}