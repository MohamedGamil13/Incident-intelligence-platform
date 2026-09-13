using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;
using Microsoft.AspNetCore.Identity;

namespace Incident_intelligence_platform.Services
{
    public class UserMangmentService
    {
        private readonly UserMangementRepo userMangementRepo;


        public UserMangmentService(UserMangementRepo userMangementRepo)
        {
            this.userMangementRepo = userMangementRepo;
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(int pageNumber, int pageSize)
        {
            return await userMangementRepo.GetAllUsersAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByType(int pageNumber, int PageSize, string role)
        {
            return await userMangementRepo.GetUsersByRoleAsync(pageNumber, PageSize, role);
        }


        public async Task<bool> DeleteUser(string id)
        {
            var user = await userMangementRepo.GetUserDataAsync(id);
            if (user == null)
            {
                return false;
            }
            return true;

        }


        public async Task<(bool Success, string Data, string ErrorMassege, IdentityResult res)> AddUserRole(string userId, string role)
        {

            var user = await userMangementRepo.GetUserDataAsync(userId);

            if (user == null)
            {
                return (false, string.Empty, "User not found");
            }
            var res = await userMangementRepo.AddUserRole(user, role);
            return (false, string.Empty, "User Role Added Successfully", res);
        }
    }
}
