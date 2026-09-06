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
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
              new Role { Id = 1, Name = "Admin" },
              new Role { Id = 2, Name = "Developer" },
              new Role { Id = 3, Name = "IncidentManager" },
              new Role { Id = 4, Name = "Viewer" }
                 );
            base.OnModelCreating(modelBuilder);
        }
    }
}
