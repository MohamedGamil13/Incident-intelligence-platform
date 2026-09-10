namespace Incident_intelligence_platform.DTOs
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}