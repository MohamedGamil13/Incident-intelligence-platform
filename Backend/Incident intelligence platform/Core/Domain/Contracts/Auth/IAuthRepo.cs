using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Domain.Contracts.Auth
{
    public interface IAuthRepo
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword);
        Task<bool> UserExistsAsync(string email);
    }
}