using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Flush.Tracing
{
    public class Tracer : ITracer
    {
        private readonly ILogger<Tracer> logger;
        private readonly IDistributedCache distributedCache;

        public Tracer(ILogger<Tracer> logger,
            IDistributedCache distributedCache)
        {
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        public async void Trace(int sessionId, Trace trace)
        {
            await distributedCache.SetAsync("somekey", null);
        }
    }
}
