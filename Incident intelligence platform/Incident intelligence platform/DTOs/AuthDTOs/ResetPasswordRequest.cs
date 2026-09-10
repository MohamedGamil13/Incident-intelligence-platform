namespace Incident_intelligence_platform.DTOs
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
