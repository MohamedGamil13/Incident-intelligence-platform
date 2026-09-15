using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Domain.Contracts.Auth
{
    public interface IUserMangementRepo
    {
        public Task<IdentityResult> AddUserAsync(ApplicationUser newUser, string password);
        public Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        public Task<ApplicationUser?> GetUserDataAsync(string userId);
        public Task<IdentityResult> UpdateUserDataAsync(ApplicationUser user);
        public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int pageNumber, int pageSize);
        public Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(int pageNumber, int pageSize, string role);
        public Task<IdentityResult> AddUserRoleAsync(ApplicationUser user, string role);
        public Task<bool> RoleExistsAsync(string roleName);
        public Task SaveChangesAsync();

    }
}
