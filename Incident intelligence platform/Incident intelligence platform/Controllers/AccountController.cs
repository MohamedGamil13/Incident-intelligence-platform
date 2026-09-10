using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.DTOs.AuthDTOs;
using Incident_intelligence_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Incident_intelligence_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto newUser)
        {
            var result = await _authService.RegisterAsync(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.Message, result.Errors, 400));
            }

            return Ok(ApiResponse<string>.SuccessResponse(message: result.Message, statusCode: 200));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest user)
        {
            var result = await _authService.LoginAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.Message, statusCode: 400));
            }

            var tokenData = new { token = result.Token, expires = result.Expiration };
            return Ok(ApiResponse<object>.SuccessResponse(tokenData, message: "Login Successful"));
        }

        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(result.Message, result.Errors, 400));
            }

            return Ok(ApiResponse<string>.SuccessResponse(message: result.Message));
        }
    }
}