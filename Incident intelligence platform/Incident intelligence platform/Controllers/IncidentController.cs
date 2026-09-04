using Incident_intelligence_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public IncidentController(AppDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetAllIncidents()
        {
            return Ok(await _context.Incidents.ToListAsync());
        }

        [HttpGet("{incidentId}")]
        public async Task<ActionResult<Incident>> GetIncident(int incidentId)
        {
            var incident = await _context.Incidents
                .SingleOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
                return NotFound();

            return Ok(incident);
        }

        [HttpPost]
        public async Task<ActionResult<Incident>> CreateIncident(Incident newIncident)
        {

            await _context.Incidents.AddAsync(newIncident);
            await _context.SaveChangesAsync();

            return Ok(newIncident);
        }

        [HttpDelete("{incidentId}")]
        public async Task<ActionResult> DeleteIncident(int incidentId)
        {
            var incident = await _context.Incidents
                .SingleOrDefaultAsync(i => i.Id == incidentId);

            if (incident == null)
                return NotFound();

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{incidentId}")]
        public async Task<ActionResult<Incident>> UpdateIncident(int incidentId, Incident newIncident)
        {
            var incident = await _context.Incidents
                .SingleOrDefaultAsync(s => s.Id == incidentId);

            if (incident == null)
                return NotFound();

            incident.Title = newIncident.Title;
            incident.Description = newIncident.Description;
            incident.Severity = newIncident.Severity;
            incident.Status = newIncident.Status;

            await _context.SaveChangesAsync();

            return Ok(incident);
        }
    }
}
