using Domain.Entities.Incidents;
using Domain.Entities.Services;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Presistance
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
        public DbSet<Service> Services { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentEvent> IncidentEvents { get; set; }
    }
}
