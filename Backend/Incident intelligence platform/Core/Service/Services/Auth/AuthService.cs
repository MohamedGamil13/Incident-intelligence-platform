using Domain.Contracts.Auth;
using Domain.Entities.Users;
using ServiceAbstraction.Contracts.Auth;
using ServiceAbstraction.Contracts.Caching;
using Shared.Dtos;
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

        public async Task<Result<string>> RegisterAsync(RegisterUserDto newUserDto)
        {
            if (await _authRepo.UserExistsAsync(newUserDto.Email))
            {
                return Result<string>.Failure("Email Already In Use");
            }

            var user = new ApplicationUser
            {
                Email = newUserDto.Email,
                UserName = newUserDto.Name
            };

            var result = await _authRepo.CreateUserAsync(user, newUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure($"User creation failed: {errors}");
            }

            return Result<string>.Success("User created successfully");
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequest loginDto)
        {
            var user = await _authRepo.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _authRepo.CheckPasswordAsync(user, loginDto.Password))
            {
                return Result<AuthResponseDto>.Failure("Invalid Email Or Password");
            }

            var roles = await _authRepo.GetRolesAsync(user);
            (string token, DateTime expiration) = _tokenService.CreateTokenAsync(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            user.RefreshTokens.Add(refreshToken);
            await _authRepo.UpdateUserAsync(user);

            var response = new AuthResponseDto
            {
                AccessToken = token,
                AccessTokenExpiration = expiration,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };

            return Result<AuthResponseDto>.Success(response);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var user = await _authRepo.GetUserByRefreshTokenAsync(dto.RefreshToken);
            if (user == null)
            {
                return Result<AuthResponseDto>.Failure("Invalid Refresh Token");
            }

            var existingToken = user.RefreshTokens.SingleOrDefault(t => t.Token == dto.RefreshToken);
            if (existingToken == null || !existingToken.IsActive)
            {
                return Result<AuthResponseDto>.Failure("Refresh Token is expired or revoked");
            }

            existingToken.RevokedOn = DateTime.UtcNow;

            var roles = await _authRepo.GetRolesAsync(user);
            (string newToken, DateTime expiration) = _tokenService.CreateTokenAsync(user, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);

            user.RefreshTokens.Add(newRefreshToken);
            await _authRepo.UpdateUserAsync(user);

            var response = new AuthResponseDto
            {
                AccessToken = newToken,
                AccessTokenExpiration = expiration,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };

            return Result<AuthResponseDto>.Success(response);
        }

        public async Task<Result<string>> RequestPasswordReset(ForgetPasswordRequest resetDto)
        {
            var user = await _authRepo.FindByEmailAsync(resetDto.Email);
            if (user == null)
            {
                return Result<string>.Failure("Invalid request or user not found");
            }

            string Otp = OtpGenerator.Generate();
            await _emailService.SendResetPasswordEmail(user.Email, Otp);
            var cacheKey = $"OTP Key N :{user.Id}";
            await _rediesCaching.SetData<string>(cacheKey, Otp);

            return Result<string>.Success("OTP has Sent To Your Email Successfully");
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest resetPassword)
        {
            var user = await _authRepo.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return Result<string>.Failure("Invalid request or user not found");
            }

            var cacheKey = $"OTP Key N :{user.Id}";
            var OtpFromDb = await _rediesCaching.GetData<string>(cacheKey);

            if (!string.IsNullOrEmpty(OtpFromDb) && OtpFromDb.Equals(resetPassword.OTP))
            {
                var result = await _authRepo.ResetPasswordAsync(user, resetPassword.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<string>.Failure($"Reset password failed: {errors}");
                }

                await _rediesCaching.DeleteData(cacheKey);
                return Result<string>.Success("Password Changed Successfully");
            }

            return Result<string>.Failure("Invalid OTP or user not found");
        }
    }
}