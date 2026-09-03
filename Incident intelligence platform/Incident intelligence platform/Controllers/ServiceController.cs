using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]

    public class ServiceController
    {
        private readonly AppDbcontext _context;
        public ServiceController(AppDbcontext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Get All Services")]
        public async Task<IEnumerable<Service>> GetAllServices()
        {
            return await _context.Services.ToListAsync();
        }
        [HttpGet]
        [Route("")]
        public async Task<Service> GetService(int servieId)
        {
            return await _context.Services.SingleOrDefaultAsync((s) => s.Id == servieId);
        }
        [HttpPost]
        [Route("")]
        public async Task CreateService(Service newService)
        {
            await _context.Services.AddAsync(newService);
        }
        [HttpDelete]
        [Route("")]
        public async Task DeleteService(Service service)
        {
            await _context.Services.ExecuteDeleteAsync((s) => s.Id == service.Id);
        }
        public async Task<Service> UpdateService(Service newService)
        {
            return newService;
        }

    }
}
