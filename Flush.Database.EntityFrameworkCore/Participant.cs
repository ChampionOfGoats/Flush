using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Participant : IParticipant
    {
        public int ParticipantId { get; set; }

        [NotMapped] public string ParticipantUniqueId => UniqueUser.ApplicationUserId;

        public DateTime? LastSeenDateTime { get; set; }

        public int? LastVote { get; set; }

        public bool IsModerator { get; set; }

        // One participant has One session.
        public int SessionId { get; set; }
        public Session Session { get; set; }

        // One participant has One unique user.
        public int UniqueUserId { get; set; }
        public UniqueUser UniqueUser { get; set; }
    }
}
