using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.AuthDTOs
{
    public class ForgetPasswordRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;


    }
}
