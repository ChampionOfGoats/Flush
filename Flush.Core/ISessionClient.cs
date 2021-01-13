using System.Threading.Tasks;

namespace Flush.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ReceiveSessionState();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PlayerEnteredSession();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PlayerLeftSession();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PlayerVoted();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SessionEnteredDiscussion();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SessionEnteredVoting();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PlayerInfoChanged();
    }
}
