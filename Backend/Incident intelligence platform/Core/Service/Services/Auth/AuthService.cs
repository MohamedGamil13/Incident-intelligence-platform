using Domain.Contracts.Auth;
using Domain.Entities.Users;
using ServiceAbstraction.Contracts.Auth;
using ServiceAbstraction.Contracts.Caching;
using Shared.Dtos.AuthDTOs;
using Shared.Helpers;

namespace ServiceLayer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IRediesCachingService _rediesCaching;

        public AuthService(IAuthRepo authRepo, ITokenService tokenService, IEmailService emailService, IRediesCachingService rediesCachingService)
        {
            _authRepo = authRepo;
            _tokenService = tokenService;
            _emailService = emailService;
            _rediesCaching = rediesCachingService;
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

        public async Task<AuthResultDto> RequestPasswordReset(ForgetPasswordRequest resetDto)
        {
            var user = await _authRepo.FindByEmailAsync(resetDto.Email);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid request or user not found" };
            }
            string Otp = OtpGenerator.Generate();
            await _emailService.SendResetPasswordEmail(user.Email, Otp);
            var cacheKey = $"OTP Key N :{user.Id}";
            await _rediesCaching.SetData<string>(cacheKey, Otp);
            return new AuthResultDto { IsSuccess = true, Message = "OTP has Sent To Your Email Successfully" };
        }
        public async Task<AuthResultDto> ResetPasswordAsync(ResetPasswordRequest resetPassword)
        {
            var user = await _authRepo.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid request or user not found" };
            }
            var cacheKey = $"OTP Key N :{user.Id}";
            var OtpFromDb = await _rediesCaching.GetData<string>(cacheKey);
            if (!string.IsNullOrEmpty(OtpFromDb) && OtpFromDb.Equals(resetPassword.OTP))
            {
                var result = await _authRepo.ResetPasswordAsync(user, resetPassword.NewPassword);
                if (!result.Succeeded)
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        Message = "Reset password failed",
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }
                await _rediesCaching.DeleteData(cacheKey);
                return new AuthResultDto { IsSuccess = true, Message = "Password Changed Successfully" };
            }
            return new AuthResultDto { IsSuccess = false, Message = "Invalid OTP or user not found" };

        }

    }
}