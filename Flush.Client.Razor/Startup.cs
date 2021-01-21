using System;
using Flush.Authentication.AspNetCoreIdentity;
using Flush.Configuration;
using Flush.Contracts;
using Flush.Database.EntityFrameworkCore;
using Flush.Server.Hubs;
using Flush.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flush.Client.Razor
{
    public class Startup
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Construct a new instance of the Startup object.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc/>
        public void ConfigureServices(IServiceCollection services)
        {
            // Populate configuration sections.
            services.Configure<ApplicationDatabaseConfiguration>(
                Configuration.GetSection(ApplicationDatabaseConfiguration.SECTION));
            services.Configure<IdentityDatabaseConfiguration>(
                Configuration.GetSection(IdentityDatabaseConfiguration.SECTION));

            // Add a current user fetch service.
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserFromHttpContext>();

            // Add Application EFCore backing components.
            services.AddApplicationDatabaseEFCore();

            // Add AspNetCore Identity authentication
            services.AddAspNetCoreIdentityAuthentication();

            // Add controllers.
            services.AddControllers();
            services.AddRazorPages()
                .AddControllersAsServices();

            // Configure SignalR.
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = false;
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            });
        }

        /// <inheritdoc/>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Map the API controllers.
                endpoints.MapControllers();

                // Map the SignalR hubs.
                endpoints.MapHub<ChatHub>(ChatHub.ENDPOINT);
                endpoints.MapHub<SessionHub>(SessionHub.ENDPOINT);
            });
        }
    }
}
