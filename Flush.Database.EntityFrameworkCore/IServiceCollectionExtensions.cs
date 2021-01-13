using Microsoft.Extensions.DependencyInjection;

namespace Flush.Database.EntityFrameworkCore
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for EF Core Application Database to the
        /// specified IServiceCollection.
        /// </summary>
        /// <returns>
        /// A reference to this instance after the operation has completed.
        /// </returns>
        public static IServiceCollection AddApplicationDatabaseEFCore(
            this IServiceCollection services)
        {
            return services
                .AddDbContext<ApplicationContext>(ServiceLifetime.Scoped)
                .AddScoped<IApplicationDatabaseProxy, ApplicationDatabaseEfCoreProxy>();
        }
    }
}
