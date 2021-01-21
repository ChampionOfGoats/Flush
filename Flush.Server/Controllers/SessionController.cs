using System.Threading.Tasks;
using Flush.Server.Hubs;
using Flush.Contracts;
using Flush.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using Flush.Models;

namespace Flush.Server.Controllers
{
    [Authorize]
    [Route("api/v2/[controller]")]
    public sealed class SessionController : ControllerBase
    {
        private static readonly IActionResult ok = new OkResult();
        private static readonly IActionResult badRequest = new BadRequestResult();

        private readonly ILogger<SessionController> logger;
        private readonly IHubContext<SessionHub, ISessionClient> hubContext;
        private readonly ICurrentUser currentUser;
        private readonly IApplicationDatabaseProxy applicationDatabaseProxy;

        /// <summary>
        /// Create a new instance of the SessionController.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hubContext">A SessionHub context.</param>
        public SessionController(ILogger<SessionController> logger,
            IHubContext<SessionHub, ISessionClient> hubContext,
            ICurrentUser currentUser,
            IApplicationDatabaseProxy applicationDatabaseProxy)
        {
            logger.LogInformation($"Initialising {nameof(SessionController)}.");

            this.logger = logger;
            this.hubContext = hubContext;
            this.currentUser = currentUser;
            this.applicationDatabaseProxy = applicationDatabaseProxy;
        }

        [HttpPost]
        [Route("vote")]
        public async Task<IActionResult> SendVote(SendVoteRequest sendVoteRequest)
        {
            logger.LogDebug($"Entered {nameof(SendVote)}.");

            if (currentUser is null)
            {
                await Task.CompletedTask;
                var exception = new NullReferenceException(nameof(currentUser));
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            var session = await applicationDatabaseProxy.GetActiveSession(currentUser.RoomUniqueId);
            if (session is null || session.Phase != (int)GamePhase.Voting)
            {
                await Task.CompletedTask;
                var exception = new InvalidOperationException("No session or bad state.");
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            logger.LogDebug($"{currentUser.UniqueId} votes {sendVoteRequest.Vote}.");
            if (!int.TryParse(sendVoteRequest.Vote, out int iVote))
            {
                await Task.CompletedTask;
                var exception = new ArgumentException(nameof(sendVoteRequest.Vote));
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            var clamped = Math.Max(iVote, (int)ModifiedFibonacciVote.Zero);
            clamped = Math.Min(clamped, (int)ModifiedFibonacciVote.Unknown);
            if (iVote != clamped)
            {
                await Task.CompletedTask;
                var exception = new ArgumentException(nameof(sendVoteRequest.Vote));
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            await applicationDatabaseProxy.SetParticipantLastVote(currentUser.RoomUniqueId, currentUser.UniqueId, iVote);
            var receiveVoteResponse = new ReceiveVoteResponse { PlayerId = currentUser.UniqueId };
            await hubContext.Clients
                .Group("ROOM_NAME_HERE")
                .ReceiveVote(receiveVoteResponse);


            logger.LogDebug($"Exiting {nameof(SendVote)}.");
            return ok;
        }

        [HttpPost]
        [Route("changerole")]
        public async Task<IActionResult> ChangeRole(ChangeRoleRequest changeRoleRequest)
        {
            logger.LogDebug($"Entered {nameof(ChangeRole)}.");

            if (currentUser is null)
            {
                await Task.CompletedTask;
                var exception = new NullReferenceException(nameof(currentUser));
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            await applicationDatabaseProxy.SetParticipantIsModerator(currentUser.RoomUniqueId, currentUser.UniqueId, changeRoleRequest.IsModerator);
            var roleChangedResponse = new RoleChangedResponse
            {
                PlayerId = currentUser.UniqueId,
                IsModerator = changeRoleRequest.IsModerator
            };
            await hubContext.Clients
                .Group("ROOM_NAME_HERE")
                .RoleChanged(roleChangedResponse);

            logger.LogDebug($"Exiting {nameof(ChangeRole)}.");
            return ok;
        }

        [HttpPost]
        [Route("transition")]
        public async Task<IActionResult> Transition(TransitionRequest transitionRequest)
        {
            logger.LogDebug($"Entered {nameof(Transition)}.");

            if (currentUser is null)
            {
                await Task.CompletedTask;
                var exception = new NullReferenceException(nameof(currentUser));
                logger.LogError(exception, string.Empty);
                return badRequest;
            }

            var phase = transitionRequest.Type == TransitionType.ToDiscussion ?
                GamePhase.Results : GamePhase.Voting;
            // applicatiomDatabaseProxy.SetSessionPhase(currentUser.Room, (int)phase);

            var transitionResponse = new TransitionResponse() { Type = transitionRequest.Type };
            await hubContext.Clients
                .Group("ROOM_NAME_HERE")
                .Transition(transitionResponse);

            logger.LogDebug($"Exiting {nameof(Transition)}.");
            return ok;
        }
    }
}
