using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Flush.Hubs.Requests;

namespace Flush.Hubs
{
    /// <summary>
    /// A SignalR Hub for receiving and relaying player messages.
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private ILogger<ChatHub> logger;

        /// <summary>
        /// Constructs a new ChatHub.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ChatHub(ILogger<ChatHub> logger)
        {
            this.logger = logger;
        }

        /// </inheritdoc>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// </inheritdoc>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Relays a message received from one player to all others.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A task representing the response.</returns>
        public async Task SendMessage(SendMessageRequest request)
        {
            await Task.CompletedTask;
        }
    }
}
