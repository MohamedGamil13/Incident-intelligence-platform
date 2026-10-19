namespace Shared.Dtos.AuthDTOs
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}