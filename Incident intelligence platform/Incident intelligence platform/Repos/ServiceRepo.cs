using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repositories
{
    public class ServiceRepository
    {
        private readonly AppDbcontext _context;

        public ServiceRepository(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Services
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
        }

        public void Delete(Service service)
        {
            _context.Services.Remove(service);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}