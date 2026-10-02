using Domain.Contracts.Auth;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Presistance.Repositories.Auth
{
    public class UserMangementRepo : IUserMangementRepo
    {
        private readonly AppDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserMangementRepo(
            AppDbcontext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
                .OrderBy(u => u.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(int pageNumber, int pageSize, string role)
        {
            var normalizedRole = role.ToUpperInvariant();

            var query = from u in _context.Users.AsNoTracking()
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where r.NormalizedName == normalizedRole
                        orderby u.UserName
                        select u;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<IdentityResult> AddUserRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }


        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}