using System.Threading.Tasks;


namespace Flush.Models
{
    /// <summary>
    /// Describes the requirements of a ChatClient.
    /// </summary>
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage chatMessage);
    }
}
