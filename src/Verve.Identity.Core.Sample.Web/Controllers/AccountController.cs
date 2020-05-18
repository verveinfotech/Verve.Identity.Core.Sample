using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Verve.Identity.Core.Sample.Web.Request;
using Verve.Identity.Core.Sample.Web.Service;

namespace Verve.Identity.Core.Sample.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(ILogger<AccountController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRegistrationRequest registrationRequest)
        {
            var registrationResponse = await _userService.Register(registrationRequest);

            if (registrationResponse != null)
            {
                return Ok(registrationResponse);
            }

            return BadRequest("Unable to register");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest loginRequest)
        {
            var loginResponse = await _userService.LoginAsync(loginRequest);

            if (loginResponse != null)
            {
                return Ok(loginResponse);
            }

            return new UnauthorizedObjectResult(new {errorMessage = "Invalid username or password"});
        }

    }
}