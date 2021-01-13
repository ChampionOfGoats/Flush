using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Flush.Server.Host
{
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Main Entrypoint.
    /// </summary>
    public class Program
    {
        /// <inheritdoc/>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            MigrateEntityFrameworkCoreDatabases(host);
            await host.RunAsync();
        }

        /// <summary>
        /// Build a new Server.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>A configured IHost.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            // Configure logging for Debug deployments.
            hostBuilder.ConfigureLogging(logging =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ==
                    Environments.Development)
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                }
            });

            // Configure server startup routine.
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();
                webBuilder.ConfigureKestrel(kestrelServerOptions =>
                {
                    kestrelServerOptions.AllowResponseHeaderCompression = false;
                });
                webBuilder.UseStartup<Startup>();
            });

            return hostBuilder;
        }

        static void MigrateEntityFrameworkCoreDatabases(IHost host)
        {
            // Create a scope during which we can request database contexts.
            using (var scope = host.Services.CreateScope())
            {
                // In parallel, attempt to migrate all registered DbContexts.
                var services = scope.ServiceProvider;
                var logger = services.GetService<ILogger<Program>>();
                var assemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .AsParallel();

                assemblies.ForAll(assembly =>
                {
                    logger.LogDebug($"Looking for contexts in {assembly}.");
                    var registeredContexts = assembly.GetTypes().Where(type =>
                    {
                        return type.IsAssignableTo(typeof(DbContext));
                    })
                    .Select(type =>
                    {
                        logger.LogDebug($"Found {type}, checking if it's registered.");
                        return services.GetService(type);
                    })
                    .OfType<DbContext>();

                    foreach (var context in registeredContexts)
                    {
                        logger.LogInformation($"Migrating {context.GetType()}.");
                        try
                        {
                            context.Database.Migrate();
                            logger.LogInformation($"Successfully migrated {context.GetType()}!");
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"Couldn't migrate {context.GetType()}.");
                        }
                    }
                });
            }
        }
    }
}
