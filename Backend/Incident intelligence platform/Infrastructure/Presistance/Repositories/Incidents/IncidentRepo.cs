using Domain.Contracts.Incidents;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using Microsoft.EntityFrameworkCore;
namespace Presistance.Repositories.Incidents
{
    public class IncidentRepository : IIncidentRepo
    {
        private readonly AppDbcontext _context;

        public IncidentRepository(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Incident>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Incidents
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Incident?> GetByIdAsync(int id)
        {
            return await _context.Incidents.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Incident incident)
        {
            await _context.Incidents.AddAsync(incident);
        }

        public void Update(Incident incident)
        {
            _context.Incidents.Update(incident);
        }

        public void Delete(Incident incident)
        {
            _context.Incidents.Remove(incident);
        }

        public async Task<bool> ServiceExistsAsync(int serviceId)
        {
            return await _context.Services.AnyAsync(s => s.Id == serviceId);
        }
        public async Task<IEnumerable<Incident>> GetActiveIncidentPerService(int serviceId, int timeWindowInMin = 5)
        {
            var targetTime = DateTime.UtcNow.AddMinutes(-timeWindowInMin);

            return await _context.Incidents
                .AsNoTracking()
                .Where(i => i.ServiceId == serviceId
                         && i.Status != IncidentStatus.Resolved
                         && i.CreatedAt >= targetTime)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetActiveIncidentCountPerService(int serviceId, int timeWindowInMin = 5)
        {
            var targetTime = DateTime.UtcNow.AddMinutes(-timeWindowInMin);

            return await _context.Incidents
                .AsNoTracking()
                .Where(i => i.ServiceId == serviceId
                         && i.Status != IncidentStatus.Resolved
                         && i.CreatedAt >= targetTime)
                .OrderByDescending(i => i.CreatedAt)
                .CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}