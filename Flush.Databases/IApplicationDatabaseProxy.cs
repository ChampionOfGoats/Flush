namespace Flush.Databases
{
    public interface IApplicationDatabaseProxy
    {
        /*
         * Room APIs
         */

        /// <summary>
        /// Get or Create a new game Room.
        /// </summary>
        /// <returns>An IRoom representing the room.</returns>
        IRoom GetOrCreateRoom();

        /// <summary>
        /// Set the name of a Room.
        /// </summary>
        void SetRoomName();

        /// <summary>
        /// Set the owner of a Room.
        /// </summary>
        void SetRoomOwner();

        /*
         * Session APIs
         */

        /// <summary>
        /// Get or Create a new game Session.
        /// </summary>
        /// <returns></returns>
        ISession GetOrStartSession();

        /// <summary>
        /// End an in-progress game Session.
        /// </summary>
        void EndSession();

        /*
         * Trace APIs
         */

        /// <summary>
        /// Record a new Trace.
        /// </summary>
        void Trace();

        /*
         * There should be something here about writing participant fun
         */
    }
}
