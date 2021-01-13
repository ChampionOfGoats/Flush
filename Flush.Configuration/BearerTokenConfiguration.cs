namespace Flush.Configuration
{
    /// <summary>
    /// Models the JwtAuthentication application settings section.
    /// </summary>
    public class BearerTokenConfiguration : ISecretKeyConfiguration
    {
        public static readonly string SECTION = @"BearerToken";

        /// <summary>
        /// The token issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The token audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Clock skew in seconds.
        /// </summary>
        public int Skew { get; set; }

        /// <inheritdoc />
        public string Key { get; set; }
    }
}
