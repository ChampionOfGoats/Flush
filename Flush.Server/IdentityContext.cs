using Flush.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Flush.Server
{
    /// <summary>
    /// ASP.NET Core Identity 5 Context.
    /// </summary>
    public class IdentityContext : IdentityDbContext<IdentityUser>
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
            var connectionString = new SqliteConnectionStringBuilder(
                identityDatabaseConfiguration.ConnectionString)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = identityDatabaseConfiguration.Key
            }.ToString();
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
