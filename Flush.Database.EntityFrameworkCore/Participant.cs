using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Participant : IParticipant
    {
        public int ParticipantId { get; set; }

        [Required]
        public string ParticipantUniqueId { get; set; }

        public DateTime? LastSeenDateTime { get; set; }

        public int? LastVote { get; set; }

        // Many-to-Many with Sessions
        public IEnumerable<SessionParticipant> ParticipantSessions { get; set; }
    }
}
