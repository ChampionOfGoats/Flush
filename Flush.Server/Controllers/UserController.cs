using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v2/[controller]")]
    public sealed class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly IAuthenticationServiceProxy authenticationServiceProxy;

        /// <summary>
        /// Create a new instance of the ChatController.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="authenticationServiceProxy">
        /// An authentication service.
        /// </param>
        public AccountController(ILogger<AccountController> logger,
            IAuthenticationServiceProxy authenticationServiceProxy)
        {
            this.logger = logger;
            this.authenticationServiceProxy = authenticationServiceProxy;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterNewAccount()
        {
            await Task.CompletedTask;
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn()
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountDetails()
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPost]
        [Route("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
