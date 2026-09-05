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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetIncidentResponseDTO>>> GetAllIncidents()
        {
            var incidents = await _context.Incidents
                .AsNoTracking()
                .ProjectToType<GetIncidentResponseDTO>()
                .ToListAsync();

            return Ok(incidents);
        }

        [HttpGet("{incidentId:int}")]
        public async Task<ActionResult<GetIncidentResponseDTO>> GetIncident(int incidentId)
        {
            var incidentDto = await _context.Incidents
                .AsNoTracking()
                .Where(i => i.Id == incidentId)
                .ProjectToType<GetIncidentResponseDTO>()
                .FirstOrDefaultAsync();

            if (incidentDto == null)
                return NotFound(new { Message = $"Incident with ID {incidentId} was not found." });

            return Ok(incidentDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetIncidentResponseDTO>> CreateIncident([FromBody] CreateIncidentRequestDTO requestDto)
        {

            var serviceExists = await _context.Services.AnyAsync(s => s.Id == requestDto.ServiceId);
            if (!serviceExists)
            {
                return BadRequest(new { Message = $"ServiceId {requestDto.ServiceId} does not exist." });
            }


            var incident = requestDto.Adapt<Incident>();


            incident.Status = IncidentStatus.Open;
            incident.CreatedAt = DateTime.UtcNow;

            await _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();

            var responseDto = incident.Adapt<GetIncidentResponseDTO>();

            return CreatedAtAction(nameof(GetIncident), new { incidentId = responseDto.Id }, responseDto);
        }

        [HttpPut("{incidentId:int}")]
        public async Task<ActionResult<GetIncidentResponseDTO>> UpdateIncident(int incidentId, [FromBody] UpdateIncidentRequestDTO requestDto)
        {
            var incident = await _context.Incidents
                .FirstOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
                return NotFound(new { Message = $"Incident with ID {incidentId} was not found." });


            requestDto.Adapt(incident);


            if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
            {
                incident.ResolvedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            var responseDto = incident.Adapt<GetIncidentResponseDTO>();
            return Ok(responseDto);
        }

        [HttpDelete("{incidentId:int}")]
        public async Task<IActionResult> DeleteIncident(int incidentId)
        {
            var incident = await _context.Incidents
                .FirstOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
                return NotFound(new { Message = $"Incident with ID {incidentId} was not found." });

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}