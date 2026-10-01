using Domain.Contracts.Incidents;
using Domain.Entities.Incidents;
using Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories
{
    public class IncidentRepo : GenericRepo<Incident, int>, IIncidentRepo
    {
        private readonly AppDbcontext _context;

        public IncidentRepo(AppDbcontext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ServiceExistsAsync(int serviceId)
        {
            return await _context.Set<Service>().AnyAsync(s => s.Id == serviceId);
        }
    }
}