using System;
using System.Threading.Tasks;
using Flush.Models;
using Flush.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Controllers
{
    /// <summary>
    /// Chat API controller.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v2/[controller]")]
    public sealed class ChatController : ControllerBase
    {
        private static readonly string NULL_MESSAGE = @"Cannot relay null message.";
        private static readonly string EMPTY_MESSAGE = @"Cannot realy empty message.";

        private readonly ILogger<ChatController> logger;
        private readonly IHubContext<ChatHub, IChatClient> hubContext;

        /// <summary>
        /// Create a new instance of the ChatController.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hubContext">A ChatHub context.</param>
        public ChatController(ILogger<ChatController> logger,
            IHubContext<ChatHub, IChatClient> hubContext)
        {
            logger.LogInformation($"Initialising {nameof(ChatController)}.");

            this.logger = logger;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// POST: api/v2/chat
        /// </summary>
        /// <param name="chatMessage">The message.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage chatMessage)
        {
            logger.LogDebug($"Entered {nameof(SendMessage)}.");

            if (chatMessage is null)
            {
                await Task.CompletedTask;
                var exception = new ArgumentNullException(NULL_MESSAGE);
                logger.LogError(exception, string.Empty);
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(chatMessage.Message))
            {
                await Task.CompletedTask;
                var exception = new ArgumentException(EMPTY_MESSAGE);
                logger.LogError(exception, string.Empty);
                return BadRequest();
            }

            await hubContext.Clients.All.ReceiveMessage(chatMessage);

            logger.LogDebug($"Exiting {nameof(SendMessage)}.");
            return Ok();
        }
    }
}
