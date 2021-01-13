using System;
using System.Threading.Tasks;
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
        private ILogger<AspNetCoreIdentityAuthenticationServiceProxy> logger;
        private readonly UserManager<IdentityUser> userManager;

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
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <inheritdoc/>
        public Task<IdentityUser> GetExistingUser()
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> SignIn()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IActionResult> SignOut()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IActionResult> RemoveUser()
        {
            throw new NotImplementedException();
        }
    }
}
