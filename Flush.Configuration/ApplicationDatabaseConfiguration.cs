namespace Flush.Configuration
{
    /// <summary>
    /// Models the Flush database options application settings
    /// section.
    /// </summary>
    public class ApplicationDatabaseConfiguration : IDatabaseConfiguration
    {
        public static readonly string SECTION = @"Application";

        /// <inheritdoc />
        public string ConnectionString { get; set; }

        /// <inheritdoc />
        public string Key { get; set; }
    }
}
