using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Session : ISession
    {
        public int SessionId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int Phase { get; set; }

        // One-to-Many with Rooms.
        public int RoomId { get; set; }

        public Room Room { get; set; }

        // Many-to-Many with Participants
        public IEnumerable<SessionParticipant> SessionParticipants { get; set; }
    }
}
