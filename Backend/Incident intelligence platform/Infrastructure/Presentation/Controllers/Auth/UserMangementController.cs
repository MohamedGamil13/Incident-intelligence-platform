using Domain.Entities.Users;
using Domain.Enums.UserMangement;
using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.Auth;

namespace Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{AppUsersRoles.Admin}")]
    public class UserMangementController : ControllerBase
    {
        private readonly IUserMangementService _userManagementService;

        public UserMangementController(IUserMangementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("PageNumber and PageSize must be greater than zero."));
            }

            var users = await _userManagementService.GetAllUsers(pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<ApplicationUser>>.SuccessResponse(users, "Users retrieved successfully"));
        }


        [HttpGet("by-role")]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string role, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Role name is required."));
            }

            var users = await _userManagementService.GetUsersByType(pageNumber, pageSize, role);
            return Ok(ApiResponse<IEnumerable<ApplicationUser>>.SuccessResponse(users, $"Users with role '{role}' retrieved successfully"));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var isDeleted = await _userManagementService.DeleteUser(id);

            if (!isDeleted)
            {
                return NotFound(ApiResponse<string>.FailureResponse("User not found or deletion failed.", statusCode: 404));
            }

            return Ok(ApiResponse<string>.SuccessResponse("User deleted successfully"));
        }


        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddUserRole(string userId, [FromQuery] string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Role cannot be empty."));
            }

            var (success, message, identityResult) = await _userManagementService.AddUserRole(userId, role);

            if (!success)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(message));
            }

            return Ok(ApiResponse<string>.SuccessResponse(message));
        }
    }
}
