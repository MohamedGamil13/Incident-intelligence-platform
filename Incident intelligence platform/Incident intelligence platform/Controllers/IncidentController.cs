using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<IncidentController> logger;

        public IncidentController(AppDbcontext context, ILogger<IncidentController> logger)
        {
            _context = context;
            this.logger = logger;
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllIncidents(int pageNumber, int pageSize)
        {
            int numberToSkip = (pageNumber - 1) * pageSize;
            var incidents = await _context.Incidents
                .AsNoTracking()
                .ProjectToType<GetIncidentResponseDTO>()
                .Skip(numberToSkip)
                .Take(pageSize)
                .ToListAsync();

            return Ok(ApiResponse<IEnumerable<GetIncidentResponseDTO>>.SuccessResponse(incidents, "Incidents retrieved successfully"));
        }

        [HttpGet("{incidentId:int}")]
        public async Task<IActionResult> GetIncident(int incidentId)
        {
            var incidentDto = await _context.Incidents
                .AsNoTracking()
                .Where(i => i.Id == incidentId)
                .ProjectToType<GetIncidentResponseDTO>()
                .FirstOrDefaultAsync();

            if (incidentDto == null)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(incidentDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] CreateIncidentRequestDTO requestDto)
        {
            var serviceExists = await _context.Services.AnyAsync(s => s.Id == requestDto.ServiceId);
            if (!serviceExists)
            {
                return BadRequest(ApiResponse<GetIncidentResponseDTO>.FailureResponse($"ServiceId {requestDto.ServiceId} does not exist."));
            }

            var incident = requestDto.Adapt<Incident>();
            incident.Status = IncidentStatus.Open;
            incident.CreatedAt = DateTime.UtcNow;

            await _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();

            var responseDto = incident.Adapt<GetIncidentResponseDTO>();
            var apiResponse = ApiResponse<GetIncidentResponseDTO>.SuccessResponse(responseDto, "Incident created successfully", 201);

            return CreatedAtAction(nameof(GetIncident), new { incidentId = responseDto.Id }, apiResponse);
        }

        [HttpPut("{incidentId:int}")]
        public async Task<IActionResult> UpdateIncident(int incidentId, [FromBody] UpdateIncidentRequestDTO requestDto)
        {
            var incident = await _context.Incidents.FirstOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
            {
                return NotFound(ApiResponse<GetIncidentResponseDTO>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            requestDto.Adapt(incident);

            if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
            {
                incident.ResolvedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            var responseDto = incident.Adapt<GetIncidentResponseDTO>();
            return Ok(ApiResponse<GetIncidentResponseDTO>.SuccessResponse(responseDto, "Incident updated successfully"));
        }

        [HttpDelete("{incidentId:int}")]
        public async Task<IActionResult> DeleteIncident(int incidentId)
        {
            var incident = await _context.Incidents.FirstOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Incident with ID {incidentId} was not found.", statusCode: 404));
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Incident deleted successfully"));
        }
    }
}