using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform
{
    public class AppDbcontext : IdentityDbContext<ApplicationUser>
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
          : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbcontext).Assembly
            );
        }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<IncidentModel> Incidents { get; set; }
        public DbSet<IncidentEvent> IncidentEvents { get; set; }
    }
}
