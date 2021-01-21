using System;
using System.Collections.Generic;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Session : ISession
    {
        public int SessionId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int Phase { get; set; }

        // One session has One room.
        public int RoomId { get; set; }
        public Room Room { get; set; }

        // One session has Many participants.
        public IEnumerable<Participant> Participants { get; set; }
    }
}
