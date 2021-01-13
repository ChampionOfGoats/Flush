using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace Flush.Database
{
    public interface IApplicationDatabaseProxy : IDisposable
    {
        /***********************************************************************
         * Room APIs
         **********************************************************************/

        /// <summary>
        /// Get or create a new game room.
        /// </summary>
        /// <param name="name">The name of the room.</param>
        /// <param name="ownerId">The owner of the room, or null.</param>
        /// <exception cref="SecurityException"></exception>
        /// <returns>An IRoom representing the room.</returns>
        Task<IRoom> GetOrCreateRoom(string name);

        /// <summary>
        /// Set the name of a room.
        /// </summary>
        /// <exception cref="SecurityException"></exception>
        /// <returns>Nothing.</returns>
        Task SetRoomName(IRoom room, string name);

        /// <summary>
        /// Set the owner of a room.
        /// </summary>
        /// <exception cref="SecurityException"></exception>
        /// <returns>Nothing.</returns>
        Task SetRoomOwner(IRoom room);

        /***********************************************************************
         * Session APIs
         **********************************************************************/

        /// <summary>
        /// Get or create a new game session.
        /// </summary>
        /// <exception cref="SecurityException"></exception>
        /// <returns>An ISession representing the session.</returns>
        Task<ISession> GetOrStartSession(IRoom room);

        /// <summary>
        /// End an in-progress game session.
        /// </summary>
        /// <exception cref="SecurityException"></exception>
        /// <returns>Nothing.</returns>
        Task EndSession(ISession session);
    }
}
