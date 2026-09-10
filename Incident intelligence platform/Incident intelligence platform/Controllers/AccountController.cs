using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Incident_intelligence_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterUserDto newUser)
        {

            //Check if User Exist
            //If not Exist => Create it Using UserManger
            var oldUser = await userManager.FindByEmailAsync(newUser.Email);
            if (oldUser != null)
            {
                return BadRequest("Email Already In Use");
            }
            ApplicationUser user = new ApplicationUser();
            user.Email = newUser.Email;
            user.UserName = newUser.Name;
            await userManager.CreateAsync(user, newUser.Password);
            return Ok(user);
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest user)
        {
            var oldUser = await userManager.FindByEmailAsync(user.Email);
            if (oldUser == null)
            {
                return BadRequest("User Not Found");
            }
            if (!await userManager.CheckPasswordAsync(oldUser, user.Password))
            {
                return BadRequest("Invalid Email Or Password");
            }
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, oldUser.Id));
            claims.Add(new Claim(ClaimTypes.Name, oldUser.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Email, oldUser.Email!));

            var userRoles = await userManager.GetRolesAsync(oldUser);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            SecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                   configuration["JWT:Key"]!
                )
            );
            SigningCredentials signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: signingCred
               );
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = DateTime.Now.AddHours(1)
            });
        }
        [HttpPut("ForgetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
        {
            ApplicationUser? userFromDb = await userManager.FindByEmailAsync(request.Email);
            if (request == null || userFromDb == null)
            {
                return BadRequest("Invalid request or user not found");
            }

            var result = userManager.ResetPasswordAsync(userFromDb, request.Token, request.NewPassword);

            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest("Password Is Weak or weak Internet Connection");
            }

            return Ok("Password Changed Successfully");



        }

    }
}
