using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
