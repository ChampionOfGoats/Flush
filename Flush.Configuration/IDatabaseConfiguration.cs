namespace Flush.Configuration
{
    /// <summary>
    /// Models a database options application settings section.
    /// </summary>
    public interface IDatabaseConfiguration : ISecretKeyConfiguration
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
