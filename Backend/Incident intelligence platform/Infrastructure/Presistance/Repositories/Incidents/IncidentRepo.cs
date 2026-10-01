using Domain.Contracts.Incidents;
using Domain.Entities.Incidents;
using Domain.Entities.Services;
using Domain.Enums.Incident;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories
{
    public class IncidentRepo : GenericRepo<Incident, int>, IIncidentRepo
    {
        private readonly AppDbcontext context;

        public IncidentRepo(AppDbcontext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> ServiceExistsAsync(int serviceId)
        {
            return await context.Set<Service>().AnyAsync(s => s.Id == serviceId);
        }

        public async Task<IEnumerable<Incident>> GetActiveIncidentPerService(int serviceId, int timeWindowInMin = 5)
        {
            var threshold = DateTime.UtcNow.AddMinutes(-timeWindowInMin);
            return await context.Set<Incident>()
                .Where(i => i.ServiceId == serviceId && i.CreatedAt >= threshold && i.Status == IncidentStatus.Open)
                .ToListAsync();
        }

        public async Task<int> GetActiveIncidentCountPerService(int serviceId, int timeWindowInMin = 5)
        {
            var threshold = DateTime.UtcNow.AddMinutes(-timeWindowInMin);
            return await context.Set<Incident>()
                .CountAsync(i => i.ServiceId == serviceId && i.CreatedAt >= threshold && i.Status == IncidentStatus.Open);
        }
    }
}