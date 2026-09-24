using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace ServiceAbstraction.Contracts.Auth
{
    public interface IUserMangementService
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsers(int pageNumber, int pageSize);
        public Task<IEnumerable<ApplicationUser>> GetUsersByType(int pageNumber, int pageSize, string role);
        public Task<bool> DeleteUser(string id);
        public Task<(bool Success, string Message, IdentityResult? IdentityResult)> AddUserRole(string userId, string role);

    }
}
