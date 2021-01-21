using System;
using Microsoft.Extensions.Logging;

namespace Flush.Extensions
{
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Logs an exception, throwing it if debug logging is enabled.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        public static void LogErrorAndThrow(this ILogger logger, Exception exception)
        {
            logger.LogError(exception, string.Empty);
            if (logger.IsEnabled(LogLevel.Debug))
            {
                throw exception;
            }
        }
    }
}
