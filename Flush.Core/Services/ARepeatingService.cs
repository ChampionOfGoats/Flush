using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace Flush.Services
{
    /// <summary>
    /// Base class for services that should autorun at fixed interval.
    /// </summary>
    public abstract class ARepeatingService : IDisposable
    {
        private static readonly int DEFAULT_INTERVAL = 30;

        private readonly ILogger logger;
        private readonly Timer timer;

        /// <summary>
        /// Create a new instance of the RepeatingService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="intervalInSeconds"></param>
        public ARepeatingService(ILogger logger, int? intervalInSeconds)
        {
            logger.LogInformation($"Initialising {nameof(ARepeatingService)}.");

            this.logger = logger;
            timer = new Timer();
            timer.Interval = (intervalInSeconds ?? DEFAULT_INTERVAL) * 1000;
            timer.AutoReset = true;
            timer.Elapsed += async (s, e) => await DoService();
            timer.Start();
        }

        /// <summary>
        /// Execute the service.
        /// </summary>
        /// <returns>Nothing.</returns>
        protected abstract Task DoService();

        /// <summary>
        /// Pause execution of the service.
        /// </summary>
        public void Pause() => timer.Stop();

        /// <summary>
        /// Resume execution of the service.
        /// </summary>
        public void Resume() => timer.Stop();

        /// <inheritdoc/>
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
