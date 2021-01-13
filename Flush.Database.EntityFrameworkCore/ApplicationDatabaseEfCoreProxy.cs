using System;
using System.Security;
using System.Threading.Tasks;
using Flush.Core;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Flush.Database.EntityFrameworkCore
{
    /// <summary>
    /// A database proxy implementing a SQLCipher-backed EFCore store.
    /// </summary>
    internal class ApplicationDatabaseEfCoreProxy : IApplicationDatabaseProxy
    {
        private static readonly string NOT_AUTHORISED = @"User is not authorised to access the data.";
        private static readonly string OWNER_IS_SET = @"The room has already been claimed.";

        private readonly ILogger<ApplicationDatabaseEfCoreProxy> logger;
        private readonly ApplicationContext context;
        private readonly IDbContextTransaction transaction;
        private readonly ICurrentUser currentUser;

        /// <summary>
        /// Create a new instance of ApplicationDatabaseEfCoreProxy
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="currentUser"></param>
        public ApplicationDatabaseEfCoreProxy(
            ILogger<ApplicationDatabaseEfCoreProxy> logger,
            ApplicationContext context,
            ICurrentUser currentUser)
        {
            logger.LogInformation($"Initialising {nameof(ApplicationDatabaseEfCoreProxy)}.");
            this.logger = logger;
            this.context = context;
            transaction = context.Database.BeginTransaction();
            this.currentUser = currentUser;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            logger.LogDebug($"Committing transaction.");
            transaction.Commit();
            logger.LogInformation($"Disposing {nameof(ApplicationDatabaseEfCoreProxy)}.");
        }

        /// <inheritdoc/>
        public async Task EndSession(ISession session)
        {
            logger.LogDebug($"Entered {nameof(EndSession)}.");

            if (!currentUser.IsAuthenticated)
            {
                // need to force asynchronous operation in unauthenticated scenarios.
                await Task.CompletedTask;
                var exception = new SecurityException(NOT_AUTHORISED);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            var theSession = session as Session;
            var utcNow = DateTime.UtcNow;
            theSession.EndDateTime = utcNow;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(EndSession)}.");
            return;
        }

        /// <inheritdoc/>
        public async Task<IRoom> GetOrCreateRoom(string name)
        {
            logger.LogDebug($"Entered {nameof(GetOrCreateRoom)}.");

            if (!currentUser.IsAuthenticated)
            {
                // need to force asynchronous operation in unauthenticated scenarios.
                await Task.CompletedTask;
                var exception = new SecurityException(NOT_AUTHORISED);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            // TODO: ApplicationDatabaseEfCoreProxy.GetOrCreateRoom
            await Task.CompletedTask;

            logger.LogDebug($"Exiting {nameof(GetOrCreateRoom)}.");
            return null;
        }

        /// <inheritdoc/>
        public async Task<ISession> GetOrStartSession(IRoom room)
        {
            logger.LogDebug($"Entered {nameof(GetOrStartSession)}.");

            if (!currentUser.IsAuthenticated)
            {
                // need to force asynchronous operation in unauthenticated scenarios.
                await Task.CompletedTask;
                var exception = new SecurityException(NOT_AUTHORISED);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            // TODO: ApplicationDatabaseEfCoreProxy.GetOrStartSession
            await Task.CompletedTask;

            logger.LogDebug($"Exiting {nameof(GetOrStartSession)}.");
            return null;
        }

        /// <inheritdoc/>
        public async Task SetRoomName(IRoom room, string name)
        {
            logger.LogDebug($"Entered {nameof(SetRoomName)}.");

            if (!currentUser.IsAuthenticated)
            {
                // need to force asynchronous operation in unauthenticated scenarios.
                await Task.CompletedTask;
                var exception = new SecurityException(NOT_AUTHORISED);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            var theRoom = room as Room;
            theRoom.Name = name;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetRoomName)}.");
        }

        /// <inheritdoc/>
        public async Task SetRoomOwner(IRoom room)
        {
            logger.LogDebug($"Entered {nameof(SetRoomOwner)}.");

            if (!currentUser.IsAuthenticated)
            {
                // need to force asynchronous operation in unauthenticated scenarios.
                await Task.CompletedTask;
                var exception = new SecurityException(NOT_AUTHORISED);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            var theRoom = room as Room;
            if (theRoom.Owner is not null)
            {
                await Task.CompletedTask;
                var exception = new InvalidOperationException(OWNER_IS_SET);
                logger.LogError(exception, string.Empty);
                throw exception;
            }

            theRoom.Owner = currentUser.Id;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetRoomOwner)}.");
        }
    }
}
