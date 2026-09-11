using Incident_intelligence_platform.DTOs.AuthDTOs;
using Incident_intelligence_platform.Models;
using Incident_intelligence_platform.Repos;

namespace Incident_intelligence_platform.Services
{
    public class AuthService
    {
        private readonly AuthRepo _authRepo;
        private readonly TokenService _tokenService;

        public AuthService(AuthRepo authRepo, TokenService tokenService)
        {
            _authRepo = authRepo;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterUserDto newUserDto)
        {
            if (await _authRepo.UserExistsAsync(newUserDto.Email))
            {
                return new AuthResultDto { IsSuccess = false, Message = "Email Already In Use" };
            }

            var user = new ApplicationUser
            {
                Email = newUserDto.Email,
                UserName = newUserDto.Name
            };

            var result = await _authRepo.CreateUserAsync(user, newUserDto.Password);
            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "User creation failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AuthResultDto { IsSuccess = true, Message = "User created successfully" };
        }

        public async Task<AuthResultDto> LoginAsync(LoginRequest loginDto)
        {
            var user = await _authRepo.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _authRepo.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid Email Or Password" };
            }

            var roles = await _authRepo.GetRolesAsync(user);
            (string token, DateTime expiration) = _tokenService.CreateTokenAsync(user, roles);

            return new AuthResultDto
            {
                IsSuccess = true,
                Message = "Login Successful",
                Token = token,
                Expiration = expiration
            };
        }

        public async Task<AuthResultDto> ResetPasswordAsync(ResetPasswordRequest resetDto)
        {
            var user = await _authRepo.FindByEmailAsync(resetDto.Email);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid request or user not found" };
            }

            var result = await _authRepo.ResetPasswordAsync(user, resetDto.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "Reset password failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AuthResultDto { IsSuccess = true, Message = "Password Changed Successfully" };
        }
    }
}