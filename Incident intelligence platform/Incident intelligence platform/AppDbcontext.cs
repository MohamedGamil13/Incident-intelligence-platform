using Incident_intelligence_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
          : base(options)
        {
        }
        public DbSet<Service> Services { get; set; }
        public DbSet<Incident> Incidents { get; set; }
    }
}
