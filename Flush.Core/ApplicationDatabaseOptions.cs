using Flush.Core.Contracts;

namespace Flush.Core
{
    /// <summary>
    /// Models the Flush database options application settings
    /// section.
    /// </summary>
    public class ApplicationDatabaseOptions : IDatabaseOptions
    {
        /// <inheritdoc />
        public string ConnectionString { get; set; }

        /// <inheritdoc />
        public string Key { get; set; }
    }
}
