using System.ComponentModel;

namespace Flush
{
    public enum TraceEventKind
    {
        [Description("A new session was started.")]
        SessionStart,
        [Description("A player has changed their role.")]
        PlayerRoleChange,
        [Description("Voting has begun.")]
        VotingStart,
        [Description("A player has issued a vote.")]
        PlayerVote,
        [Description("Discussion has begun.")]
        DiscussionStart,
        [Description("A session was finished.")]
        SessionFinish
    }
}
