using Flush.Configuration;
using Microsoft.Data.SqlClient;
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
        /// The applications rooms.
        /// </summary>
        /// <remarks>
        /// Room 1 <-> N Session
        /// Room 1 <-> 0..1 UniqueUser
        /// </remarks>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// The play sessions.
        /// </summary>
        /// <remarks>
        /// Session 1 <-> N Participant
        /// Session N <-> 1 Room
        /// </remarks>
        public DbSet<Session> Sessions { get; set; }

        /// <summary>
        /// The participants of sessions.
        /// </summary>
        /// <remarks>
        /// Participant N <-> 1 UniqueUser
        /// Participant N <-> 1 Session
        /// </remarks>
        public DbSet<Participant> Participants { get; set; }

        /// <summary>
        /// The anonymised unique users.
        /// </summary>
        /// <remarks>
        /// UniqueUser 1 <-> N Participant
        /// UniqueUser 1 <-> 0..N Room
        /// </remarks>
        public DbSet<UniqueUser> UniqueUsers { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqlConnectionStringBuilder(
                applicationDatabaseConfiguration.ConnectionString)
            {
                // TODO: These are development creds for an ephemeral SQL Server '19 Express container on my local machine. Replace with config.
                UserID = "SA",
                Password = "V3ryStr0nk_"
            }.ToString();
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
