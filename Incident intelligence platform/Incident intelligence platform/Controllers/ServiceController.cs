using Incident_intelligence_platform.DTOs;
using Mapster;
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
        public async Task<ActionResult<IEnumerable<GetServiceResponseDTO>>> GetAllServices()
        {
            var services = await _context.Services
                .AsNoTracking()
                .ProjectToType<GetServiceResponseDTO>()
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("{serviceId:int}")]
        public async Task<ActionResult<GetServiceResponseDTO>> GetService(int serviceId)
        {
            var serviceDto = await _context.Services
                .AsNoTracking()
                .Where(s => s.Id == serviceId)
                .ProjectToType<GetServiceResponseDTO>()
                .FirstOrDefaultAsync();

            if (serviceDto == null)
                return NotFound(new { Message = $"Service with ID {serviceId} was not found." });

            return Ok(serviceDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetServiceResponseDTO>> CreateService([FromBody] CreateServiceRequestDTO requestDto)
        {

            var service = requestDto.Adapt<Service>();

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();


            var responseDto = service.Adapt<GetServiceResponseDTO>();

            return CreatedAtAction(nameof(GetService), new { serviceId = responseDto.Id }, responseDto);
        }

        [HttpPut("{serviceId:int}")]
        public async Task<ActionResult<GetServiceResponseDTO>> UpdateService(int serviceId, [FromBody] UpdateServiceRequestDTO requestDto)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound(new { Message = $"Service with ID {serviceId} was not found." });


            requestDto.Adapt(service);

            await _context.SaveChangesAsync();

            var responseDto = service.Adapt<GetServiceResponseDTO>();
            return Ok(responseDto);
        }

        [HttpDelete("{serviceId:int}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound(new { Message = $"Service with ID {serviceId} was not found." });

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}