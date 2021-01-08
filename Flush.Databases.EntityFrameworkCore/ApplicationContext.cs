using Flush.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Flush.Databases.EntityFrameworkCore
{
    /// <summary>
    /// An EF Core based data model context for the Flush game.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private readonly ApplicationDatabaseOptions _databaseOptions;

        /// <summary>
        /// Construct a new instance of the ApplicationContext object.
        /// </summary>
        /// <param name="databaseOptions">The database options.</param>
        /// <param name="options">The dbcontext options.</param>
        public ApplicationContext(IOptions<ApplicationDatabaseOptions> databaseOptions,
            DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            _databaseOptions = databaseOptions.Value;
        }

        /// <summary>
        /// The game rooms.
        /// </summary>
        public DbSet<Room> Room { get; set; }

        /// <summary>
        /// The game sessions.
        /// </summary>
        public DbSet<Session> Sessions { get; set; }

        /// <summary>
        /// The session traces..
        /// </summary>
        public DbSet<Trace> Traces { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder(
                _databaseOptions.ConnectionString)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = _databaseOptions.Key
            }.ToString();
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
