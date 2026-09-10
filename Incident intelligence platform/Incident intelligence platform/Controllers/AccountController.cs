using Incident_intelligence_platform.DTOs;
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
        public async Task<ActionResult> Register(RegisterUserDto newUser)
        {
            var result = await _authService.RegisterAsync(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest user)
        {
            var result = await _authService.LoginAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { token = result.Token, expires = result.Expiration });
        }

        [HttpPut("ForgetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result.Message);
        }
    }
}