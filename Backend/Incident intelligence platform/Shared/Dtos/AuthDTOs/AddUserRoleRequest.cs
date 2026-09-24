using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.AuthDTOs
{
    public class AddUserRoleRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [MaxLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string RoleName { get; set; } = string.Empty;
    }
}