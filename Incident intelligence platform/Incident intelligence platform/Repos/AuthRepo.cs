using Incident_intelligence_platform.Enums;
using Incident_intelligence_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repos
{
    public class AuthRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {

            var result = await _userManager.CreateAsync(user, password);


            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, AppUsersRoles.Viewer);


            }

            return result;
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }
    }
}