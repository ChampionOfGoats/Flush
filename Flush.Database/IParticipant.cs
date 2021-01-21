using System;
namespace Flush.Database
{
    public interface IParticipant
    {
        public string ParticipantUniqueId { get; }
        public DateTime? LastSeenDateTime { get; }
        public int? LastVote { get; }
        public bool IsModerator { get; }
    }
}
