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
/* {
    "Issuer": "https://localhost:7145",
    "Audience": "https://localhost:7145",
    "AccessKeyLifeTimeInMin": 5,
    "RefershTokenLifeTimeInDays" :  30 , 
    "SigningKey ": "T+iAT4+XXDOc79Xl6JJ8+q0U9wzXa6ec86QkNxaqatE="
  }, */