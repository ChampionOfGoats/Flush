using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flush.Database
{
    /// <summary>
    /// Describes the functionality required by the application database.
    /// </summary>
    public interface IApplicationDatabaseProxy : IDisposable
    {
        //
        // CREATE
        //

        Task<IRoom> CreateRoom(string roomName);
        Task<ISession> CreateSession(string roomUniqueId);
        Task<IParticipant> CreateParticipant(string roomUniqueId, string applicationUserUniqueId);

        //
        // READ
        //

        Task<IRoom> GetRoom(string roomUniqueId);
        Task<ISession> GetActiveSession(string roomUniqueId);
        Task<IParticipant> GetParticipant(string roomUniqueId, string applicationUserUniqueId);
        Task<IEnumerable<IParticipant>> GetParticipants(string roomUniqueId);

        //
        // UPDATE
        //

        Task SetRoomName(string roomUniqueId, string roomName);
        Task SetRoomOwner(string roomUniqueId, string applicationUserUniqueId);
        Task SetParticipantLastSeen(string roomUniqueId, string applicationUserUniqueId);
        Task SetParticipantLastVote(string roomUniqueId, string applicationUserUniqueId, int vote);
        Task SetParticipantIsModerator(string roomUniqueId, string applicationUserUniqueId, bool isModerator);

        //
        // DELETE
        //

        Task EndActiveSession(string roomId);
    }
}
