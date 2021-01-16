using System.Threading.Tasks;
using Flush.Models;

namespace Flush.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionClient
    {
        /// <summary>
        /// Invoked on the caller when they first connect to the hub.
        /// </summary>
        /// <param name="receiveSessionResponse">The response.</param>
        Task ReceiveSession(ReceiveSessionResponse receiveSessionResponse);

        /// <summary>
        /// Invoked on the group when someone connects to the hub.
        /// </summary>
        /// <param name="playerConnectedResponse">The response.</param>
        Task PlayerConnected(PlayerConnectedResponse playerConnectedResponse);

        /// <summary>
        /// Invoked on the group when someone disconnects from the hub.
        /// </summary>
        /// <param name="playerDisconnectedResponse">The response.</param>
        Task PlayerDisconnected(PlayerDisconnectedResponse playerDisconnectedResponse);

        /// <summary>
        /// Invoked on the group when someone is disconnected for a set period.
        /// </summary>
        /// <param name="playerRemovedResponse">The response.</param>
        Task PlayerRemoved(PlayerRemovedResponse playerRemovedResponse);

        /// <summary>
        /// Invoked on the group when someone votes.
        /// </summary>
        /// <param name="receiveVoteResponse">The response.</param>
        Task ReceiveVote(ReceiveVoteResponse receiveVoteResponse);

        /// <summary>
        /// Invoked on the group when a moderator progresses the game.
        /// </summary>
        /// <param name="transitionResponse">The response.</param>
        Task Transition(TransitionResponse transitionResponse);

        /// <summary>
        /// Invoked on the group when a player changes their role.
        /// </summary>
        /// <param name="roleChangedResponse">The response.</param>
        Task RoleChanged(RoleChangedResponse roleChangedResponse);
    }
}
