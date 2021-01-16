using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Flush.Contracts;
using Flush.Models;
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
        private static readonly string NOT_LOGGED_IN = @"User is not logged-in.";
        private static readonly string INVALID_TOKEN = @"An invalid token was presented by the client.";

        private readonly ILogger<AccountController> logger;
        private readonly IAuthenticationServiceProxy authenticationServiceProxy;
        private readonly ICurrentUser currentUser;

        /// <summary>
        /// Create a new instance of the ChatController.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="authenticationServiceProxy">
        /// An authentication service.
        /// </param>
        public AccountController(ILogger<AccountController> logger,
            IAuthenticationServiceProxy authenticationServiceProxy,
            ICurrentUser currentUser)
        {
            this.logger = logger;
            this.authenticationServiceProxy = authenticationServiceProxy;
            this.currentUser = currentUser;
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
        public async Task<IActionResult> SignIn(SignInRequest signInRequest)
        {
            var claims = new List<Claim>
            {
                new Claim("displayname", signInRequest.DisplayName),
                new Claim("room", signInRequest.Room)
            };

            return await authenticationServiceProxy.SignIn(
                new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountDetails()
        {
            // Cannot get the details a user if they are not signed in.
            if (currentUser is null)
            {
                await Task.CompletedTask;
                var exception = new InvalidOperationException(NOT_LOGGED_IN);
                logger.LogError(exception, string.Empty);
                return BadRequest();
            }

            // Todo: Return account object
            return Ok();
        }

        [HttpPost]
        [Route("signout")]
        public new async Task<IActionResult> SignOut()
        {
            // Cannot sign-out a user if they are not signed in.
            if (currentUser is null)
            {
                await Task.CompletedTask;
                var exception = new InvalidOperationException(NOT_LOGGED_IN);
                logger.LogError(exception, string.Empty);
                return BadRequest();
            }

            // Cannot sign-out a user that does not exist.
            var validUser = authenticationServiceProxy.GetExistingUser(string.Empty);
            if (validUser is null)
            {
                await Task.CompletedTask;
                var exception = new InvalidOperationException(INVALID_TOKEN);
                logger.LogError(exception, string.Empty);
                return BadRequest();
            }

            // If user is a guest, delete them.
            // Else, sign them out.

            return Ok();
        }
    }
}
