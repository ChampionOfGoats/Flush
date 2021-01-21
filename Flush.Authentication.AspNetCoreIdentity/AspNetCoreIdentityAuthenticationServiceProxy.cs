using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Flush.Extensions;
using Flush.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flush.Authentication.AspNetCoreIdentity
{
    /// <summary>
    /// AuthenticationServiceProxy wrapping the ASP.NET Core Identity framework.
    /// </summary>
    internal sealed class AspNetCoreIdentityAuthenticationServiceProxy
        : IAuthenticationServiceProxy
    {
        private static readonly OkResult ok = new OkResult();

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
            logger.LogDebug($"Initialising {nameof(AspNetCoreIdentityAuthenticationServiceProxy)}.");

            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <inheritdoc/>
        public async Task<UserInfo> GetUserByEmail(string emailAddress)
        {
            logger.LogDebug($"Entering {nameof(GetUserByEmail)}.");

            var applicationUser = await userManager.FindByEmailAsync(emailAddress);
            if (applicationUser is null)
            {
                var exception = new NullReferenceException(nameof(applicationUser));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetUserByEmail)}.");
            return new UserInfo
            {
                UniqueId = applicationUser.Id,
                DisplayName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };
        }

        /// <inheritdoc/>
        public async Task<CanSignInResult> CanSignIn(UserInfo userInfo)
        {
            logger.LogDebug($"Entering {nameof(CanSignIn)}.");

            var applicationUser = await userManager.FindByIdAsync(userInfo.UniqueId);
            if (applicationUser is null)
            {
                var exception = new NullReferenceException(nameof(applicationUser));
                logger.LogErrorAndThrow(exception);
                return CanSignInResult.NotFound;
            }

            var canSignInResult = CanSignInResult.Success;
            var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(applicationUser);
            if (signInManager.IsSignedIn(claimsPrincipal))
            {
                canSignInResult = CanSignInResult.SignedIn;
            }
            else if (applicationUser.LockoutEnabled && applicationUser.LockoutEnd > DateTime.UtcNow)
            {
                canSignInResult = CanSignInResult.LockedOut;
            }

            logger.LogDebug($"Exiting {nameof(CanSignIn)}.");
            return canSignInResult;
        }

        /// <inheritdoc/>
        public Task<UserInfo> RegisterUser(RegistrationData registrationData)
        {
            logger.LogDebug($"Entering {nameof(RegisterUser)}.");

            throw new NotImplementedException();

            logger.LogDebug($"Exiting {nameof(RegisterUser)}.");
        }

        /// <inheritdoc/>
        public async Task<UserInfo> SignIn(SignInCredentials signInCredentials)
        {
            logger.LogDebug($"Entering {nameof(SignIn)}.");

            var applicationUser = await userManager.FindByNameAsync(signInCredentials.Username);
            if (applicationUser is null)
            {
                var exception = new NullReferenceException(nameof(applicationUser));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            if (!await userManager.CheckPasswordAsync(applicationUser, signInCredentials.Password))
            {
                logger.LogWarning($"Unsuccessful sign-in as {signInCredentials.Username} attempted by " +
                                  $"{signInManager.Context.Connection.RemoteIpAddress?.ToString()}.");
                return null;
            }

            // generate claims.
            var claims = new Claim[]
            {
                new Claim(ClaimType.UserIdentifier.Description(), applicationUser.Id),
                new Claim(ClaimType.FirstName.Description(), applicationUser.FirstName),
                new Claim(ClaimType.LastName.Description(), applicationUser.LastName),
                new Claim(ClaimType.RoomUniqueId.Description(), signInCredentials.Room)
            };

            await signInManager.SignInWithClaimsAsync(applicationUser, false, claims);

            logger.LogDebug($"Exiting {nameof(SignIn)}.");
            return new UserInfo
            {
                UniqueId = applicationUser.Id,
                DisplayName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };
        }

        /// <inheritdoc/>
        public async Task<bool> SignOut()
        {
            logger.LogDebug($"Entering {nameof(SignOut)}.");

            await signInManager.SignOutAsync();

            logger.LogDebug($"Exiting {nameof(SignOut)}.");
            return true;
        }

        /// <inheritdoc/>
        public Task<bool> RemoveUser(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
