using System.Collections.Generic;
using System.Threading.Tasks;
using Flush.Server.Hubs;
using Flush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v2/[controller]")]
    public sealed class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> logger;
        private readonly IHubContext<SessionHub, ISessionClient> hubContext;

        /// <summary>
        /// Create a new instance of the SessionController.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hubContext">A SessionHub context.</param>
        public SessionController(ILogger<SessionController> logger,
            IHubContext<SessionHub, ISessionClient> hubContext)
        {
            logger.LogInformation($"Initialising {nameof(SessionController)}.");

            this.logger = logger;
            this.hubContext = hubContext;
        }
        // GET: api/v2/session
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            await Task.CompletedTask;
            return new string[] { "value1", "value2" };
        }

        // GET api/v2/session/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            await Task.CompletedTask;
            return "value";
        }

        // POST api/v2/session
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await Task.CompletedTask;
        }

        // PUT api/v2/session/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            await Task.CompletedTask;
        }

        // DELETE api/v2/session/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await Task.CompletedTask;
        }
    }
}
