using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.AuthDTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
