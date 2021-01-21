using Microsoft.Extensions.DependencyInjection;

namespace Flush.Server
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
        public static IServiceCollection AddApplicationServer(
            this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
    }
}
