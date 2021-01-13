namespace Flush.Models
{
    /// <summary>
    /// A single message sent via the Chat API.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// The name of the user who sent the message.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The message.
        /// </summary>
        public string Message { get; set; }
    }
}
