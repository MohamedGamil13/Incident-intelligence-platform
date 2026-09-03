using Incident_intelligence_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform
{
    public class AppDbcontext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=IncidentIntelligenceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Service> Services { get; set; }
        public DbSet<Incident> Incidents { get; set; }
    }
}
