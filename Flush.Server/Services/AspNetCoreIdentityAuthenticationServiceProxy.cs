using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Services
{
    /// <summary>
    /// AuthenticationServiceProxy wrapping the ASP.NET Core Identity framework.
    /// </summary>
    public sealed class AspNetCoreIdentityAuthenticationServiceProxy
        : IAuthenticationServiceProxy
    {
        private static readonly string NON_EXISTENT_USER = @"User does not exist.";

        private static readonly OkResult ok = new OkResult();
        private static readonly BadRequestResult badRequest = new BadRequestResult();

        private readonly ILogger<AspNetCoreIdentityAuthenticationServiceProxy> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        /// <summary>
        /// Create a new instance of the
        /// AspNetCoreIdentityAuthenticationServiceProxy.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userManager">
        /// The ASP.NET Core Identity UserManager.
        /// </param>
        public AspNetCoreIdentityAuthenticationServiceProxy(
            ILogger<AspNetCoreIdentityAuthenticationServiceProxy> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetExistingUser(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        /// <inheritdoc/>
        public Task<IActionResult> RegisterNewUser()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IActionResult> RegisterGuestUser()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IActionResult> SignIn(ClaimsPrincipal claimsPrincipal)
        {
            await signInManager.Context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);
            return ok;
        }

        /// <inheritdoc/>
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return ok;
        }

        /// <inheritdoc/>
        public async Task<IActionResult> RemoveUser(string userId)
        {
            // Try to find the user.
            var user = await GetExistingUser(userId);
            if (user is null)
            {
                var exception = new InvalidOperationException(NON_EXISTENT_USER);
                logger.LogError(exception, string.Empty);
                return new BadRequestResult();
            }

            // Sign the user out.
            await signInManager.SignOutAsync();

            // Delete the user.
            // This has the effect of invalidating tokens, too.
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded ? ok : badRequest;
        }
    }
}
