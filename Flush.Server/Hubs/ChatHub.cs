using System;
using System.Threading.Tasks;
using Flush.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Flush.Server.Hubs
{
    /// <summary>
    /// SignalR auto-generated implementation of IChatClient hub.
    /// </summary>
    [Authorize]
    public sealed class ChatHub : Hub<IChatClient>
    {
        public static readonly string ENDPOINT = $@"/ws/{nameof(ChatHub).ToLowerInvariant()}";

        /// <inheritdoc/>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <inheritdoc/>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
