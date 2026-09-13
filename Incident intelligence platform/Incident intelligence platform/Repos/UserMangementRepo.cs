using Incident_intelligence_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repos
{
    public class UserMangementRepo
    {
        private readonly AppDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserMangementRepo(AppDbcontext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IdentityResult> AddUserAsync(ApplicationUser newUser, string password)
        {
            return await _userManager.CreateAsync(newUser, password);
        }


        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }


        public async Task<ApplicationUser?> GetUserDataAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        public async Task<IdentityResult> UpdateUserDataAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);

        }


        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(int pageNumber, int pageSize, string role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);

            return usersInRole
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<IdentityResult> AddUserRole(ApplicationUser user, string role)
        {

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}