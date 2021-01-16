using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flush.Contracts;
using Flush.Extensions;
using Flush.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Flush.Database.EntityFrameworkCore
{
    /// <summary>
    /// A database proxy implementing a SQLCipher-backed EFCore store.
    /// </summary>
    internal class ApplicationDatabaseEfCoreProxy : IApplicationDatabaseProxy
    {
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
            this.currentUser = currentUser;

            transaction = context.Database.BeginTransaction();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            logger.LogDebug($"Committing transaction.");
            transaction.Commit();
            logger.LogInformation($"Disposing {nameof(ApplicationDatabaseEfCoreProxy)}.");
        }

        //
        // CREATE
        //

        /// <inheritdoc/>
        public async Task<IRoom> CreateRoom(string roomName)
        {
            logger.LogDebug($"Entering {nameof(CreateRoom)}.");

            if (string.IsNullOrWhiteSpace(roomName))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomName));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var room = new Room
            {
                RoomUniqueId = Guid.NewGuid().ToString(),
                Name = roomName
            };
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(CreateRoom)}.");
            return room;
        }

        /// <inheritdoc/>
        public async Task<ISession> CreateSession(string roomUniqueId)
        {
            logger.LogDebug($"Entering {nameof(CreateSession)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var sessionQuery = context.Sessions
                .Include(s => s.Room)
                .Where(s => s.Room.RoomUniqueId == roomUniqueId);
            logger.LogDebug($"{sessionQuery.ToQueryString()}");

            var session = await sessionQuery.SingleOrDefaultAsync();
            if (session is null)
            {
                var roomQuery = context.Rooms
                    .Where(r => r.RoomUniqueId == roomUniqueId);
                logger.LogDebug($"{roomQuery.ToQueryString()}");

                var room = await roomQuery.SingleOrDefaultAsync();
                if (room is null)
                {
                    var exception = new NullReferenceException(nameof(room));
                    logger.LogErrorAndThrow(exception);
                    return null;
                }

                session = new Session()
                {
                    StartDateTime = DateTime.UtcNow,
                    Phase = (int)GamePhase.Voting,
                    RoomId = room.RoomId
                };

                await context.Sessions.AddAsync(session);
                await context.SaveChangesAsync();
            }

            logger.LogDebug($"Exiting {nameof(CreateSession)}.");
            return session;
        }

        /// <inheritdoc/>
        public async Task<IParticipant> CreateParticipant(string roomUniqueId, string applicationUserUniqueId)
        {
            logger.LogDebug($"Entering {nameof(CreateParticipant)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            if (string.IsNullOrWhiteSpace(applicationUserUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(applicationUserUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var participantQuery = context.Participants
                .Where(p => p.ParticipantUniqueId == applicationUserUniqueId);
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participant = await participantQuery.SingleOrDefaultAsync();
            if (participant is null)
            {
                // In creating a new participant, we should link it to the active session.
                // HOWEVER, we should only proceed if there is indeed an active session.

                var sessionQuery = context.Sessions
                    .Include(s => s.Room)
                    .Where(s => s.Room.RoomUniqueId == roomUniqueId)
                    .Where(s => s.EndDateTime == null);
                logger.LogDebug($"{sessionQuery.ToQueryString()}");

                var session = await sessionQuery.SingleOrDefaultAsync();
                if (session is null)
                {
                    var exception = new NullReferenceException(nameof(session));
                    logger.LogErrorAndThrow(exception);
                    return null;
                }

                participant = new Participant
                {
                    ParticipantUniqueId = applicationUserUniqueId,
                };

                var sessionParticipant = new SessionParticipant
                {
                    Session = session,
                    Participant = participant
                };

                await context.Participants.AddAsync(participant);
                await context.SessionParticipants.AddAsync(sessionParticipant);
                await context.SaveChangesAsync();
            }

            // TODO: Do we want to enforce session linkage here? Or is that too presumptious...

            logger.LogDebug($"Exiting {nameof(CreateParticipant)}.");
            return participant;
        }

        //
        // READ
        //

        /// <inheritdoc/>
        public async Task<IRoom> GetRoom(string roomUniqueId)
        {
            logger.LogDebug($"Entering {nameof(GetRoom)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var roomQuery = context.Rooms
                .Where(r => r.RoomUniqueId == roomUniqueId);
            logger.LogDebug($"{roomQuery.ToQueryString()}");

            var room = await roomQuery.SingleOrDefaultAsync();
            if (room is null)
            {
                var exception = new NullReferenceException(nameof(room));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetRoom)}.");
            return room;
        }

        /// <inheritdoc/>
        public async Task<ISession> GetActiveSession(string roomUniqueId)
        {
            logger.LogDebug($"Entering {nameof(GetActiveSession)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var sessionQuery = context.Sessions
                .Include(s => s.Room)
                .Where(s => s.Room.RoomUniqueId == roomUniqueId)
                .Where(s => s.EndDateTime == null);
            logger.LogDebug($"{sessionQuery.ToQueryString()}");

            var session = await sessionQuery.SingleOrDefaultAsync();
            if (session is null)
            {
                var exception = new NullReferenceException(nameof(session));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetActiveSession)}.");
            return session;
        }

        /// <inheritdoc/>
        public async Task<IParticipant> GetParticipant(string roomUniqueId, string applicationUserUniqueId)
        {
            logger.LogDebug($"Entering {nameof(GetParticipant)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            if (string.IsNullOrWhiteSpace(applicationUserUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(applicationUserUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var participantQuery = context.Participants
                .Where(p => p.ParticipantUniqueId == applicationUserUniqueId);
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participant = await participantQuery.SingleOrDefaultAsync();
            if (participant is null)
            {
                var exception = new NullReferenceException(nameof(participant));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetParticipant)}.");
            return participant;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IParticipant>> GetParticipants(string roomUniqueId)
        {
            logger.LogDebug($"Entering {nameof(GetParticipants)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            var participantQuery = context.Sessions
                .Include(s => s.Room)
                .Where(s => s.Room.RoomUniqueId == roomUniqueId)
                .SelectMany(s => s.SessionParticipants)
                .Select(sp => sp.Participant)
                .Distinct();
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participants = await participantQuery.ToListAsync();
            if (participants is null)
            {
                var exception = new NullReferenceException(nameof(participants));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetParticipants)}.");
            return participants;
        }

        //
        // UPDATE
        //

        /// <inheritdoc/>
        public async Task SetRoomName(string roomUniqueId, string roomName)
        {
            logger.LogDebug($"Entering {nameof(SetRoomName)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            if (string.IsNullOrWhiteSpace(roomName))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomName));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var roomQuery = context.Rooms
                .Where(r => r.RoomUniqueId == roomUniqueId);
            logger.LogDebug($"{roomQuery.ToQueryString()}");

            var room = await roomQuery.SingleOrDefaultAsync();
            if (room is null)
            {
                var exception = new NullReferenceException(nameof(room));
                logger.LogErrorAndThrow(exception);
                return;
            }

            room.Name = roomName;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetRoomName)}.");
            return;
        }

        /// <inheritdoc/>
        public async Task SetRoomOwner(string roomUniqueId, string applicationUserUniqueId)
        {
            logger.LogDebug($"Entering {nameof(SetRoomOwner)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            if (string.IsNullOrWhiteSpace(applicationUserUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(applicationUserUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var roomQuery = context.Rooms
                .Where(r => r.RoomUniqueId == roomUniqueId)
                .Where(r => r.OwnerUniqueId == applicationUserUniqueId);
            logger.LogDebug($"{roomQuery.ToQueryString()}");

            var room = await roomQuery.SingleOrDefaultAsync();
            if (room is null)
            {
                var exception = new NullReferenceException(nameof(room));
                logger.LogErrorAndThrow(exception);
                return;
            }

            room.OwnerUniqueId = applicationUserUniqueId;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetRoomOwner)}.");
            return;
        }

        /// <inheritdoc/>
        public async Task SetParticipantLastSeen(string roomUniqueId, string applicationUserUniqueId)
        {
            logger.LogDebug($"Entering {nameof(SetParticipantLastSeen)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            if (string.IsNullOrWhiteSpace(applicationUserUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(applicationUserUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var participantQuery = context.Participants
                .Where(p => p.ParticipantUniqueId == applicationUserUniqueId);
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participant = await participantQuery.SingleOrDefaultAsync();
            if (participant is null)
            {
                var exception = new NullReferenceException(nameof(participant));
                logger.LogErrorAndThrow(exception);
                return;
            }

            participant.LastSeenDateTime = DateTime.UtcNow;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetParticipantLastSeen)}.");
            return;
        }

        /// <inheritdoc/>
        public async Task SetParticipantLastVote(string roomUniqueId, string applicationUserUniqueId, int vote)
        {
            logger.LogDebug($"Entering {nameof(SetParticipantLastVote)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            if (string.IsNullOrWhiteSpace(applicationUserUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(applicationUserUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var participantQuery = context.Participants
                .Where(p => p.ParticipantUniqueId == applicationUserUniqueId);
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participant = await participantQuery.SingleOrDefaultAsync();
            if (participant is null)
            {
                var exception = new NullReferenceException(nameof(participant));
                logger.LogErrorAndThrow(exception);
                return;
            }

            participant.LastVote = vote;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetParticipantLastVote)}.");
            return;
        }

        //
        // DELETE
        //

        /// <inheritdoc/>
        public async Task EndActiveSession(string roomUniqueId)
        {
            logger.LogDebug($"Entering {nameof(EndActiveSession)}.");

            if (string.IsNullOrWhiteSpace(roomUniqueId))
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(nameof(roomUniqueId));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var sessionQuery = context.Sessions
                .Include(s => s.Room)
                .Where(s => s.Room.RoomUniqueId == roomUniqueId)
                .Where(s => s.EndDateTime == null);
            logger.LogDebug($"{sessionQuery.ToQueryString()}");

            var session = await sessionQuery.SingleOrDefaultAsync();
            if (session is null)
            {
                var exception = new NullReferenceException(nameof(session));
                logger.LogErrorAndThrow(exception);
                return;
            }

            session.EndDateTime = DateTime.UtcNow;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(EndActiveSession)}.");
            return;
        }
    }
}
