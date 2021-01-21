using Flush.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Flush.Authentication.AspNetCoreIdentity
{
    /// <summary>
    /// ASP.NET Core Identity 5 Context.
    /// </summary>
    internal class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IdentityDatabaseConfiguration identityDatabaseConfiguration;

        /// <summary>
        /// Create a new instance of the IdentityContext.
        /// </summary>
        /// <param name="identityDatabaseConfiguration">
        /// The configuration.
        /// </param>
        /// <param name="options">The options.</param>
        public IdentityContext(
            IOptions<IdentityDatabaseConfiguration> identityDatabaseConfiguration,
            DbContextOptions<IdentityContext> options)
            : base(options)
        {
            this.identityDatabaseConfiguration = identityDatabaseConfiguration.Value;
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqlConnectionStringBuilder(
                identityDatabaseConfiguration.ConnectionString)
            {
                // TODO: These are development creds for an ephemeral SQL Server '19 Express container on my local machine. Replace with config.
                UserID = "SA",
                Password = "V3ryStr0nk_"
            }.ToString();
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
