using System;
using Flush.Configuration;
using Flush.Contracts;
using Flush.Database.EntityFrameworkCore;
using Flush.Server.Hubs;
using Flush.Server.Services;
using Flush.Tracing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flush.Server.Host
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

            // Add Identity.
            services.AddDbContext<IdentityContext>();
            var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();
            identityBuilder.AddEntityFrameworkStores<IdentityContext>();

            // Add authentication proxy.
            services.AddScoped<IAuthenticationServiceProxy, AspNetCoreIdentityAuthenticationServiceProxy>();

            // Add Authentication.
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = false;
            });
            authenticationBuilder.AddCookie(options =>
            {
            });

            // Add tracing.
            services.AddScoped<ITracer, Tracer>();
            services.AddStackExchangeRedisCache(options =>
            {
                // todo: configure redis
            });

            // Add controllers.
            services.AddControllers();

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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // Map a 'hello' endpoint
                endpoints.MapGet("/", async context =>
                {
                    await context.Response
                        .WriteAsync("Hello, is it me you're looking for?");
                });

                // Map the API controllers.
                endpoints.MapControllers();

                // Map the SignalR hubs.
                endpoints.MapHub<ChatHub>(ChatHub.ENDPOINT);
                endpoints.MapHub<SessionHub>(SessionHub.ENDPOINT);
            });
        }
    }
}
