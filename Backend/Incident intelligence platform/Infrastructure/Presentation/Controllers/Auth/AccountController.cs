using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Contracts.Auth;
using Shared.Dtos.AuthDTOs;
using System.Security.Claims;

namespace Presentation.Controllers.Auth
{
    public class AccountController : ApiController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto newUser)
        {
            var result = await _authService.RegisterAsync(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.ErrorMessage!, statusCode: 400));
            }

            return Ok(ApiResponse<string>.SuccessResponse(result.Data!, message: "User registered successfully", statusCode: 200));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequest user)
        {
            var result = await _authService.LoginAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.ErrorMessage!, statusCode: 400));
            }

            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result.Data!, message: "Login Successful"));
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.ErrorMessage!, statusCode: 400));
            }

            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result.Data!, message: "Token Refreshed Successfully"));
        }

        [HttpPut("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.ErrorMessage!, statusCode: 400));
            }

            return Ok(ApiResponse<string>.SuccessResponse(result.Data!, message: "Password Reset Successfully"));
        }

        [HttpPut("ForgetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authService.RequestPasswordReset(request);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.ErrorMessage!, statusCode: 400));
            }

            return Ok(ApiResponse<string>.SuccessResponse(result.Data!, message: "OTP sent successfully"));
        }

        [HttpGet("my-roles")]
        [Authorize]
        public ActionResult GetMyRolesFromToken()
        {
            var roles = User.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value)
                            .ToList();

            return Ok(ApiResponse<IEnumerable<string>>.SuccessResponse(roles, "User roles retrieved from token successfully"));
        }
    }
}