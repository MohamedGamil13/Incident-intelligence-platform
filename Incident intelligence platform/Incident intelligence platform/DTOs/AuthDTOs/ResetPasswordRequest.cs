namespace Incident_intelligence_platform.DTOs.AuthDTOs
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }


    }
}
