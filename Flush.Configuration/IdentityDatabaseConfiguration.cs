namespace Flush.Configuration
{
    /// <summary>
    /// Models the ASP.NET Core Identity database options application settings
    /// section.
    /// </summary>
    public class IdentityDatabaseConfiguration :  IDatabaseConfiguration
    {
        public static readonly string SECTION = @"Identity";

        /// <inheritdoc />
        public string ConnectionString { get; set; }

        /// <inheritdoc />
        public string Key { get; set; }
    }
}
