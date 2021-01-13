using Flush.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Flush.Database.EntityFrameworkCore
{
    /// <summary>
    /// An EF Core based data model context for the Flush game.
    /// </summary>
    internal class ApplicationContext : DbContext
    {
        private readonly ApplicationDatabaseConfiguration applicationDatabaseConfiguration;

        /// <summary>
        /// Construct a new instance of the ApplicationContext object.
        /// </summary>
        /// <param name="applicationDatabaseConfiguration">
        /// The database options.
        /// </param>
        /// <param name="options">The dbcontext options.</param>
        public ApplicationContext(
            IOptions<ApplicationDatabaseConfiguration> applicationDatabaseConfiguration,
            DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            this.applicationDatabaseConfiguration = applicationDatabaseConfiguration.Value;
        }

        /// <summary>
        /// The game rooms.
        /// </summary>
        public DbSet<Room> Room { get; set; }

        /// <summary>
        /// The game sessions.
        /// </summary>
        public DbSet<Session> Sessions { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder(
                applicationDatabaseConfiguration.ConnectionString)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = applicationDatabaseConfiguration.Key
            }.ToString();
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
