using Shared.Dtos;
using Shared.Dtos.AuthDTOs;
namespace ServiceAbstraction.Contracts.Auth
{
    public interface IAuthService
    {
        Task<Result<string>> RegisterAsync(RegisterUserDto newUserDto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginRequest loginDto);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task<Result<string>> RequestPasswordReset(ForgetPasswordRequest resetDto);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest resetPassword);
    }
}
