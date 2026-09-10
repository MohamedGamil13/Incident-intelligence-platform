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

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllServices(int pageNumber, int pageSize)
        {
            var services = await _context.Services
                .AsNoTracking()
                .ProjectToType<GetServiceResponseDTO>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(ApiResponse<IEnumerable<GetServiceResponseDTO>>.SuccessResponse(services, "Services retrieved successfully"));
        }

        [HttpGet("{serviceId:int}")]
        public async Task<IActionResult> GetService(int serviceId)
        {
            var serviceDto = await _context.Services
                .AsNoTracking()
                .Where(s => s.Id == serviceId)
                .ProjectToType<GetServiceResponseDTO>()
                .FirstOrDefaultAsync();

            if (serviceDto == null)
            {
                return NotFound(ApiResponse<GetServiceResponseDTO>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            return Ok(ApiResponse<GetServiceResponseDTO>.SuccessResponse(serviceDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequestDTO requestDto)
        {
            var service = requestDto.Adapt<Service>();

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            var responseDto = service.Adapt<GetServiceResponseDTO>();
            var apiResponse = ApiResponse<GetServiceResponseDTO>.SuccessResponse(responseDto, "Service created successfully", 201);

            return CreatedAtAction(nameof(GetService), new { serviceId = responseDto.Id }, apiResponse);
        }

        [HttpPut("{serviceId:int}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] UpdateServiceRequestDTO requestDto)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound(ApiResponse<GetServiceResponseDTO>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            requestDto.Adapt(service);
            await _context.SaveChangesAsync();

            var responseDto = service.Adapt<GetServiceResponseDTO>();
            return Ok(ApiResponse<GetServiceResponseDTO>.SuccessResponse(responseDto, "Service updated successfully"));
        }

        [HttpDelete("{serviceId:int}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Service with ID {serviceId} was not found.", statusCode: 404));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Service deleted successfully"));
        }
    }
}