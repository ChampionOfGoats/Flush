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

        /// <summary>
        /// Create a new instance of ApplicationDatabaseEfCoreProxy
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public ApplicationDatabaseEfCoreProxy(
            ILogger<ApplicationDatabaseEfCoreProxy> logger,
            ApplicationContext context)
        {
            logger.LogInformation($"Initialising {nameof(ApplicationDatabaseEfCoreProxy)}.");
            this.logger = logger;
            this.context = context;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
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
                    Room = room
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

            var sessionQuery = context.Sessions
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

            var uniqueUserQuery = context.UniqueUsers
                .Where(uu => uu.ApplicationUserId == applicationUserUniqueId);
            logger.LogDebug($"{uniqueUserQuery.ToQueryString()}");

            var uniqueUser = await uniqueUserQuery.SingleOrDefaultAsync();
            if (uniqueUserQuery is null)
            {
                uniqueUser = new UniqueUser
                {
                    ApplicationUserId = applicationUserUniqueId
                };

                await context.UniqueUsers.AddAsync(uniqueUser);
                await context.SaveChangesAsync();
            }

            var participantQuery = context.Participants
                .Where(p => p.SessionId == session.SessionId)
                .Where(p => p.UniqueUserId == uniqueUser.UniqueUserId);
            logger.LogDebug($"{participantQuery.ToQueryString()}");

            var participant = await participantQuery.SingleOrDefaultAsync();
            if (participant is null)
            {
                participant = new Participant
                {
                    Session = session,
                    UniqueUser = uniqueUser,
                    IsModerator = false
                };

                await context.Participants.AddAsync(participant);
                await context.SaveChangesAsync();
            }

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
            logger.LogDebug("Test");
            logger.LogDebug($"YOUR MOTHER{roomQuery.ToQueryString()}");

            var room = await roomQuery.FirstOrDefaultAsync();
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

            var sessionQuery = context.Sessions
                .Where(s => s.Room.RoomUniqueId == roomUniqueId)
                .Where(s => s.EndDateTime == null)
                .Include(s => s.Participants);
            logger.LogDebug($"{sessionQuery.ToQueryString()}");

            var session = await sessionQuery.SingleOrDefaultAsync();
            if (session is null)
            {
                var exception = new NullReferenceException(nameof(session));
                logger.LogErrorAndThrow(exception);
                return null;
            }

            logger.LogDebug($"Exiting {nameof(GetParticipants)}.");
            return session.Participants;
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
                .Where(r => r.UniqueUserId == null);
            logger.LogDebug($"{roomQuery.ToQueryString()}");

            var room = await roomQuery.SingleOrDefaultAsync();
            if (room is null)
            {
                var exception = new NullReferenceException(nameof(room));
                logger.LogErrorAndThrow(exception);
                return;
            }

            var uniqueUserQuery = context.UniqueUsers
                .Where(uu => uu.ApplicationUserId == applicationUserUniqueId);
            logger.LogDebug($"{uniqueUserQuery.ToQueryString()}");

            var uniqueUser = await uniqueUserQuery.SingleOrDefaultAsync();
            if (uniqueUser is null)
            {
                var exception = new NullReferenceException(nameof(uniqueUser));
                logger.LogErrorAndThrow(exception);
                return;
            }

            room.UniqueUser = uniqueUser;
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

        /// <inheritdoc/>
        public async Task SetParticipantIsModerator(string roomUniqueId, string applicationUserUniqueId, bool isModerator)
        {
            logger.LogDebug($"Entering {nameof(SetParticipantIsModerator)}.");

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

            participant.IsModerator = isModerator;
            await context.SaveChangesAsync();

            logger.LogDebug($"Exiting {nameof(SetParticipantIsModerator)}.");
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
