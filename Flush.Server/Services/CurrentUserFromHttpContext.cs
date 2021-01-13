using System;
using System.Security.Claims;
using Flush.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Services
{
    /// <summary>
    /// A service for querying information about the currently logged in user.
    /// </summary>
    public class CurrentUserFromHttpContext : ICurrentUser, IDisposable
    {
        private readonly ILogger<CurrentUserFromHttpContext> logger;
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Get a value indicating the authentication status of the user.
        /// </summary>
        public bool IsAuthenticated =>
            context.HttpContext.User?.Identity.IsAuthenticated ?? false;

        /// <summary>
        /// Gets the primary key GUID of the logged in user.
        /// </summary>
        public string Id =>
            context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

        /// <summary>
        /// Gets the first name of the logged in user.
        /// </summary>
        public string FirstName =>
            context.HttpContext.User?.FindFirst(ClaimTypes.GivenName).Value;

        /// <summary>
        /// Gets the last name of the logged in user.
        /// </summary>
        public string LastName =>
            context.HttpContext.User?.FindFirst(ClaimTypes.Surname).Value;

        /// <summary>
        /// Gets the full name of the logged in user.
        /// </summary>
        public string FullName =>
            $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Gets the email address of the logged in user.
        /// </summary>
        public string Email =>
            context.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;

        /// <summary>
        /// Gets the username of the logged in user.
        /// </summary>
        public string UserName =>
            context.HttpContext.User?.FindFirst(ClaimTypes.Name).Value;

        /// <summary>
        /// Gets the room that the user has logged in to.
        /// </summary>
        public string CurrentRoom =>
            context.HttpContext.User?.FindFirst(ClaimTypes.UserData).Value;

        /// <summary>
        /// Creates a new instance of the CurrentUserService.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The HTTP context accessor.</param>
        public CurrentUserFromHttpContext(ILogger<CurrentUserFromHttpContext> logger,
            IHttpContextAccessor context)
        {
            logger.LogInformation($"Initialising {nameof(CurrentUserFromHttpContext)}.");

            this.logger = logger;
            this.context = context;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            logger.LogInformation($"Disposing {nameof(CurrentUserFromHttpContext)}.");
        }
    }
}
