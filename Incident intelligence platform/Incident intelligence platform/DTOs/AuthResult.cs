namespace Incident_intelligence_platform.DTOs
{
    public class AuthResultDto : ApiResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}