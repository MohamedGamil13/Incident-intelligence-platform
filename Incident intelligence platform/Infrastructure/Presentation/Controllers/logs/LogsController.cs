using Domain.Enums.UserMangement;
using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.Logs;
using Shared.Dtos.Logs;

namespace Presentation.Controllers.Logs
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateLog([FromBody] CreateLogDto dto)
        {
            long latencyTime = dto.LatencyMs;
            if (HttpContext.Items.TryGetValue("RequestLatencyMs", out var itemVal) && itemVal is long elapsed)
            {
                latencyTime = elapsed;
            }

            var traceId = Guid.Parse(HttpContext.TraceIdentifier);
            var result = await _logsService.CreateLogAsync(dto, traceId, LatancyTime: (long)latencyTime);
            if (!result.Success)
            {
                return NotFound(ApiResponse<LogResponseDto>.FailureResponse(result.ErrorMessage!, statusCode: 404));
            }
            var apiResponse = ApiResponse<LogResponseDto>.SuccessResponse(result.Data!, "Log ingested successfully", 201);
            return CreatedAtAction(nameof(GetLogById), new { logId = result.Data!.Id }, apiResponse);
        }

        [HttpGet]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> GetAllLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var result = await _logsService.GetAllLogsAsync(pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<LogResponseDto>>.SuccessResponse(result.Data, "Logs retrieved successfully"));
        }

        [HttpGet("{logId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> GetLogById(int logId)
        {
            var result = await _logsService.GetLogByIdAsync(logId);
            if (!result.Success)
            {
                return NotFound(ApiResponse<LogResponseDto>.FailureResponse(result.ErrorMessage!, statusCode: 404));
            }

            return Ok(ApiResponse<LogResponseDto>.SuccessResponse(result.Data!));
        }

        [HttpGet("service/{serviceId:int}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> GetLogsByService(int serviceId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var result = await _logsService.GetLogsByServiceIdAsync(serviceId, pageNumber, pageSize);
            if (!result.Success)
            {
                return NotFound(ApiResponse<IEnumerable<LogResponseDto>>.FailureResponse(result.ErrorMessage!, statusCode: 404));
            }

            return Ok(ApiResponse<IEnumerable<LogResponseDto>>.SuccessResponse(result.Data, "Service logs retrieved successfully"));
        }

        [HttpGet("trace/{traceId:guid}")]
        [Authorize(Roles = $"{AppUsersRoles.Admin},{AppUsersRoles.IncidentManager},{AppUsersRoles.Developer}")]
        public async Task<ActionResult> GetLogsByTraceId(Guid traceId)
        {
            var result = await _logsService.GetLogsByTraceIdAsync(traceId);
            return Ok(ApiResponse<IEnumerable<LogResponseDto>>.SuccessResponse(result.Data, "Trace logs retrieved successfully"));
        }
    }
}