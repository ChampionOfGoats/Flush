using System.Collections.Generic;

namespace Flush.Database.EntityFrameworkCore
{
    internal class UniqueUser
    {
        public int UniqueUserId { get; set; }
        public string ApplicationUserId { get; set; }

        // One unique user may have Many rooms.
        public IEnumerable<Room> Rooms { get; set; }

        // One unique user has Many participants.
        public IEnumerable<Participant> Participants { get; set; }
    }
}
