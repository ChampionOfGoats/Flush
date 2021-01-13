using System;
using System.Text;
using System.Threading.Tasks;
using Flush.Configuration;
using Flush.Core;
using Flush.Database.EntityFrameworkCore;
using Flush.Server.Hubs;
using Flush.Server.Services;
using Flush.Tracing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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
            var identityBuilder = services.AddIdentity<IdentityUser, IdentityRole>();
            identityBuilder.AddEntityFrameworkStores<IdentityContext>();

            // Add authentication proxy.
            services.AddScoped<IAuthenticationServiceProxy,
                AspNetCoreIdentityAuthenticationServiceProxy>();

            // Add Authentication.
            var bearerTokenConfiguration = Configuration
                .GetSection(BearerTokenConfiguration.SECTION)
                .Get<BearerTokenConfiguration>();
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            });

            // Add Bearer Tokens.
            authenticationBuilder.AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.Configuration = new OpenIdConnectConfiguration();

                // Configure validation from file.
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = bearerTokenConfiguration.Issuer,
                    ValidAudience = bearerTokenConfiguration.Audience,
                    ClockSkew = TimeSpan.FromSeconds(bearerTokenConfiguration.Skew),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.Default.GetBytes(bearerTokenConfiguration.Key)),
                };

                // Configure event pipeline to receive tokens.
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = async (messageReceivedContext) =>
                    {
                        var token = messageReceivedContext
                            .Request
                            .Query["access_token"];
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            messageReceivedContext.Token = token;
                        }
                        await Task.CompletedTask;
                    }
                };
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
