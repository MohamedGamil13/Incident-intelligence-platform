using Microsoft.AspNetCore.Mvc;

namespace Incident_intelligence_platform.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        public void SignIn() { }
        public void SignUp() { }
        public void ResetPassword() { }
        public void RefereshToken() { }

    }
}
