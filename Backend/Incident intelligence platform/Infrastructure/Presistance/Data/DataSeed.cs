using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Entities.Logs;
using Domain.Entities.ServiceDeployments;
using Domain.Entities.Services;
using Domain.Entities.Users;
using Domain.Enums.UserMangement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Presistance.Data
{
    public class DataSeeding : IDataSeed
    {
        private readonly AppDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeding(
            AppDbcontext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await _context.Database.MigrateAsync();
                }

                var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
                var seedDataPath = Path.Combine(baseDirectory, "Data", "DataSeed");

                bool hasChanges = false;


                if (!await _context.Set<Service>().AnyAsync())
                {
                    var filePath = Path.Combine(seedDataPath, "services.json");
                    if (File.Exists(filePath))
                    {
                        await using var jsonStream = File.OpenRead(filePath);
                        var services = await JsonSerializer.DeserializeAsync<List<Service>>(jsonStream);

                        if (services is not null && services.Count > 0)
                        {
                            await _context.Set<Service>().AddRangeAsync(services);
                            hasChanges = true;

                            await _context.SaveChangesAsync();
                        }
                    }
                }

                // 2. Seed Incidents
                if (!await _context.Set<Incident>().AnyAsync())
                {
                    var filePath = Path.Combine(seedDataPath, "incidents.json");
                    if (File.Exists(filePath))
                    {
                        await using var jsonStream = File.OpenRead(filePath);
                        var incidents = await JsonSerializer.DeserializeAsync<List<Incident>>(jsonStream);

                        if (incidents is not null && incidents.Count > 0)
                        {
                            await _context.Set<Incident>().AddRangeAsync(incidents);
                            hasChanges = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                // 3. Seed Incident Events
                if (!await _context.Set<IncidentEvent>().AnyAsync())
                {
                    var filePath = Path.Combine(seedDataPath, "incident_events.json");
                    if (File.Exists(filePath))
                    {
                        await using var jsonStream = File.OpenRead(filePath);
                        var incidentEvents = await JsonSerializer.DeserializeAsync<List<IncidentEvent>>(jsonStream);

                        if (incidentEvents is not null && incidentEvents.Count > 0)
                        {
                            await _context.Set<IncidentEvent>().AddRangeAsync(incidentEvents);
                            hasChanges = true;
                        }
                    }
                }

                // 4. Seed Logs
                if (!await _context.Set<Log>().AnyAsync())
                {
                    var filePath = Path.Combine(seedDataPath, "logs.json");
                    if (File.Exists(filePath))
                    {
                        await using var jsonStream = File.OpenRead(filePath);
                        var logs = await JsonSerializer.DeserializeAsync<List<Log>>(jsonStream);

                        if (logs is not null && logs.Count > 0)
                        {
                            await _context.Set<Log>().AddRangeAsync(logs);
                            hasChanges = true;
                        }
                    }
                }

                // 5. Seed Service Deployments
                if (!await _context.Set<ServiceDeployment>().AnyAsync())
                {
                    var filePath = Path.Combine(seedDataPath, "service_deployments.json");
                    if (File.Exists(filePath))
                    {
                        await using var jsonStream = File.OpenRead(filePath);
                        var deployments = await JsonSerializer.DeserializeAsync<List<ServiceDeployment>>(jsonStream);

                        if (deployments is not null && deployments.Count > 0)
                        {
                            await _context.Set<ServiceDeployment>().AddRangeAsync(deployments);
                            hasChanges = true;
                        }
                    }
                }

                if (hasChanges)
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred during data seeding: {ex.Message}", ex);
            }
        }

        public async Task SeedIdentityDataAsync()
        {
            // Seed Application Roles defined in AppUsersRoles enum
            if (!_roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole(AppUsersRoles.Admin),
                    new IdentityRole(AppUsersRoles.IncidentManager),
                    new IdentityRole(AppUsersRoles.Developer),
                    new IdentityRole(AppUsersRoles.Viewer)
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            // Seed Initial Default Users
            if (!_userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@platform.com",
                    EmailConfirmed = true,
                    PhoneNumber = "01102535450"
                };

                var incidentManagerUser = new ApplicationUser
                {
                    UserName = "incidentmanager",
                    Email = "manager@platform.com",
                    EmailConfirmed = true,
                    PhoneNumber = "01102535451"
                };
                var Developer = new ApplicationUser
                {
                    UserName = "Developer",
                    Email = "Developer@platform.com",
                    EmailConfirmed = true,
                    PhoneNumber = "01102535452"
                };
                var Viewer = new ApplicationUser
                {
                    UserName = "Viewer",
                    Email = "Viewer@platform.com",
                    EmailConfirmed = true,
                    PhoneNumber = "01102535452"
                };

                await _userManager.CreateAsync(adminUser, "Password123@");
                await _userManager.CreateAsync(incidentManagerUser, "Password123@");
                await _userManager.CreateAsync(Developer, "Password123@");
                await _userManager.CreateAsync(Viewer, "Password123@");

                await _userManager.AddToRoleAsync(adminUser, AppUsersRoles.Admin);
                await _userManager.AddToRoleAsync(incidentManagerUser, AppUsersRoles.IncidentManager);
                await _userManager.AddToRoleAsync(Developer, AppUsersRoles.Developer);
                await _userManager.AddToRoleAsync(Viewer, AppUsersRoles.Viewer);
            }
        }
    }
}
