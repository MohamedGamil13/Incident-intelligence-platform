namespace Incident_intelligence_platform.Configurations
{
    public class JwtOptions
    {
        public string SigningKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string AccessKeyLifeTimeInMin { get; set; } = string.Empty;
        public string RefershTokenLifeTimeInDays { get; set; } = string.Empty;

    }
}
