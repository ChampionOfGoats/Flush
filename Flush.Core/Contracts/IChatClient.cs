using System.Threading.Tasks;
using Flush.Models;

namespace Flush.Contracts
{
    /// <summary>
    /// Describes the requirements of a ChatClient.
    /// </summary>
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage chatMessage);
    }
}
