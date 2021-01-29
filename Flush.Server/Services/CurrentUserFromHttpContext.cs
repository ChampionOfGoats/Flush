using System;
using Flush.Contracts;
using Flush.Models;
using Flush.Extensions;
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

        /// <inheritdoc/>
        public string DisplayName =>
            // TODO: Needs to use full name claim (not set yet)
            context.HttpContext.User?.FindFirst(ClaimType.FirstName.Description()).Value;

        /// <inheritdoc/>
        public string UniqueId =>
            context.HttpContext.User?.FindFirst(ClaimType.UserIdentifier.Description()).Value;

        /// <inheritdoc/>
        public string RoomUniqueId =>
            context.HttpContext.User?.FindFirst(ClaimType.RoomUniqueId.Description()).Value;

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
