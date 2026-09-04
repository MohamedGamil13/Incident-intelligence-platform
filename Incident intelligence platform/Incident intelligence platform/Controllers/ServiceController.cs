using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public ServicesController(AppDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices()
        {
            return Ok(await _context.Services.ToListAsync());
        }

        [HttpGet("{serviceId}")]
        public async Task<ActionResult<Service>> GetService(int serviceId)
        {
            var service = await _context.Services
                .SingleOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<Service>> CreateService(Service newService)
        {

            await _context.Services.AddAsync(newService);
            await _context.SaveChangesAsync();

            return Ok(newService);
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services
                .SingleOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{serviceId}")]
        public async Task<ActionResult<Service>> UpdateService(
            int serviceId,
            Service updatedService)
        {
            var service = await _context.Services
                .SingleOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound();

            service.Name = updatedService.Name;
            service.Description = updatedService.Description;

            await _context.SaveChangesAsync();

            return Ok(service);
        }
    }
}