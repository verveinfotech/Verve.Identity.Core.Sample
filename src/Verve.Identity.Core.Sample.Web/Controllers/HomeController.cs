using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Verve.Identity.Core.Sample.Web.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new { applicationName = "Test Identity" });
        }

        [HttpGet("secret/admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminSecret()
        {
            return Ok(new { secretInfo = "This is Admin Secret" });
        }

        [HttpGet("secret/user")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetUserSecret()
        {
            return Ok(new { secretInfo = "This is User Secret" });
        }


        [HttpGet("public")]
        public IActionResult GetPublicInfo()
        {
            return Ok(new { secretInfo = "This is not a secret Secret" });
        }

    }
}
