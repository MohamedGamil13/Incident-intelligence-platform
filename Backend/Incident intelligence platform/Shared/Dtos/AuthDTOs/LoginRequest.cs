using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.AuthDTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; } = string.Empty;
    }
}