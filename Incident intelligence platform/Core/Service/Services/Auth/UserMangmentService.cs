using Domain.Contracts.Auth;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using ServiceAbstraction.Contracts.Auth;

namespace Service.Services.Auth
{
    public class UserMangmentService : IUserMangementService
    {
        private readonly IUserMangementRepo _userMangementRepo;

        public UserMangmentService(IUserMangementRepo userMangementRepo)
        {
            _userMangementRepo = userMangementRepo;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(int pageNumber, int pageSize)
        {
            return await _userMangementRepo.GetAllUsersAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByType(int pageNumber, int pageSize, string role)
        {
            return await _userMangementRepo.GetUsersByRoleAsync(pageNumber, pageSize, role);
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _userMangementRepo.GetUserDataAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = await _userMangementRepo.DeleteUserAsync(user);
            return result.Succeeded;
        }

        public async Task<(bool Success, string Message, IdentityResult? IdentityResult)> AddUserRole(string userId, string role)
        {

            var user = await _userMangementRepo.GetUserDataAsync(userId);
            if (user == null)
            {
                return (false, "User not found", null);
            }


            var roleExists = await _userMangementRepo.RoleExistsAsync(role);
            if (!roleExists)
            {
                return (false, $"Role '{role}' does not exist in the system", null);
            }


            var result = await _userMangementRepo.AddUserRoleAsync(user, role);

            if (!result.Succeeded)
            {

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, result);
            }

            return (true, "User Role Added Successfully", result);
        }
    }
}