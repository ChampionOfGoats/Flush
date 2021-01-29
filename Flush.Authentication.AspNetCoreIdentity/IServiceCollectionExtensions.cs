using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Flush.Authentication.AspNetCoreIdentity
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for Flush to use AspNetCore Identity to
        /// the specified IServiceCollection.
        /// </summary>
        /// <returns>
        /// A reference to this instance after the operation has completed.
        /// </returns>
        public static IServiceCollection AddAspNetCoreIdentityAuthentication(this IServiceCollection services)
        {
            // Add Identity.
            services.AddDbContext<IdentityContext>();
            var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();
            identityBuilder.AddEntityFrameworkStores<IdentityContext>();

            // Add authentication proxy.
            services.AddScoped<IAuthenticationServiceProxy, AspNetCoreIdentityAuthenticationServiceProxy>();

            // Add Authentication.
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.RequireAuthenticatedSignIn = false;
            });

            // Set cookie options.
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Auth/Login";
                //options.SlidingExpiration = true;
            });

            // Add the authentication proxy.
            services.AddScoped<IAuthenticationServiceProxy, AspNetCoreIdentityAuthenticationServiceProxy>();

            return services;
        }
    }
}
