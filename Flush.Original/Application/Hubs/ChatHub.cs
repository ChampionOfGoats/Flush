using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Flush.Application.Hubs.Requests;
using System.Security.Claims;
using Flush.Extensions;
using Flush.Application.Hubs.Responses;
using Flush.Services;

namespace Flush.Application.Hubs
{
    /// <summary>
    /// A SignalR Hub for receiving and relaying player messages.
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private ILogger<ChatHub> logger;
        private CurrentUserService currentUserService;

        private string Room => Context.User
            .FindFirst(ClaimTypes.NameIdentifier)?
            .GetFlushRoom();

        /// <summary>
        /// Constructs a new ChatHub.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ChatHub(ILogger<ChatHub> logger,
            CurrentUserService currentUserService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        /// </inheritdoc>
        public override async Task OnConnectedAsync()
        {
            logger.LogDebug($"Enter {nameof(OnConnectedAsync)}");
            await Groups.AddToGroupAsync(Context.ConnectionId, Room);
            logger.LogDebug($"Exit {nameof(OnConnectedAsync)}");
        }

        /// </inheritdoc>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            logger.LogDebug($"Enter {nameof(OnDisconnectedAsync)}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Room);
            logger.LogDebug($"Exit {nameof(OnDisconnectedAsync)}");
        }

        /// <summary>
        /// Relays a message received from one player to all others.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A task representing the response.</returns>
        public async Task SendMessage(SendMessageRequest request)
        {
            logger.LogDebug($"Enter {nameof(SendMessage)}");
            await Clients.OthersInGroup(Room)
                .SendAsync("ReceiveMessage", new SendMessagesResponse()
                {
                    Player = Context.User
                        .FindFirst(ClaimTypes.NameIdentifier)?
                        .GetFlushUsername(),
                    Message = request.Message
                });
            logger.LogDebug($"Exit {nameof(SendMessage)}");
        }
    }
}
