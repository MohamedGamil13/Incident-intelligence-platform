using Domain.Contracts.Incident;
using Incident_intelligence_platform;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories.Incident
{
    public class IncidentRepository : IIncidentRepo
    {
        private readonly AppDbcontext _context;

        public IncidentRepository(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Incident.Incident>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Incidents
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Incident.Incident?> GetByIdAsync(int id)
        {
            return await _context.Incidents.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Domain.Entities.Incident.Incident incident)
        {
            await _context.Incidents.AddAsync(incident);
        }

        public void Update(Domain.Entities.Incident.Incident incident)
        {
            _context.Incidents.Update(incident);
        }

        public void Delete(Domain.Entities.Incident.Incident incident)
        {
            _context.Incidents.Remove(incident);
        }

        public async Task<bool> ServiceExistsAsync(int serviceId)
        {
            return await _context.Services.AnyAsync(s => s.Id == serviceId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}