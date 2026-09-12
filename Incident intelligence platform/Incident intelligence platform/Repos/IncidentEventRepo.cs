using Incident_intelligence_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repos
{
    public class IncidentEventRepo
    {
        private readonly AppDbcontext appDbcontext;

        public IncidentEventRepo(AppDbcontext appDbcontext)
        {
            this.appDbcontext = appDbcontext;
        }

        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLineAsync(
            int incidentId,
            int pageSize,
            int pageNumber)
        {
            return await appDbcontext.IncidentEvents
                .AsNoTracking()
                .Where(e => e.IncidentId == incidentId)
                .OrderBy(e => e.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddEventAsync(IncidentEvent newEvent)
        {
            await appDbcontext.IncidentEvents.AddAsync(newEvent);

            await appDbcontext.SaveChangesAsync();
        }

        public async Task<bool> CheckIncidentExistAsync(int incidentId)
        {
            return await appDbcontext.Incidents
                .AnyAsync(i => i.Id == incidentId);
        }
    }
}