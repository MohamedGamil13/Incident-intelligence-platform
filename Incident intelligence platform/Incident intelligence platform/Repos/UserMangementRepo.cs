using Incident_intelligence_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repos
{
    public class UserMangementRepo
    {
        private readonly AppDbcontext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserMangementRepo(AppDbcontext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task addUser(ApplicationUser newUser)
        {
            await context.Users.AddAsync(newUser);
        }
        public void DeleteUser(ApplicationUser user)
        {
            context.Users.Remove(user);
        }
        public async Task<ApplicationUser?> GetUserData(string userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public void UpdateUserData(ApplicationUser newUserData)
        {
            context.Users.Update(newUserData);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUser(int pageNumber, int pageSize)
        {
            return await context.Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUser(int pageNumber, int pageSize, string role)
        {

            var usersInRole = await userManager.GetUsersInRoleAsync(role);

            return usersInRole
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
