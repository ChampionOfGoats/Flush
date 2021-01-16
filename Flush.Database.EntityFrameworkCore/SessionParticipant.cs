using System;
using System.ComponentModel.DataAnnotations;

namespace Flush.Database.EntityFrameworkCore
{
    internal class SessionParticipant
    {
        public int SessionId { get; set; }

        public Session Session { get; set; }

        public int ParticipantId { get; set; }

        public Participant Participant { get; set; }
    }
}
