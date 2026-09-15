using Incident_intelligence_platform.DTOs.AuthDTOs;
namespace ServiceAbstraction.Contracts.Auth
{
    public interface IAuthService
    {
        public Task<AuthResultDto> RegisterAsync(RegisterUserDto newUserDto);
        public Task<AuthResultDto> LoginAsync(LoginRequest loginDto);
        public Task<AuthResultDto> ResetPasswordAsync(ResetPasswordRequest resetDto);

    }
}
