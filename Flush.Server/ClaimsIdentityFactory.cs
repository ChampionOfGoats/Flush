using System.Security.Claims;
using Flush.Authentication;
using Flush.Models;
using Flush.Extensions;

namespace Flush.Server
{
    /// <summary>
    /// Factory for the creation of new Claims Identities
    /// </summary>
    public static class ClaimsIdentityFactory
    {
        /// <summary>
        /// Create a <see cref="ClaimsIdentity"/> from the given <see cref="applicationUser"/>.
        /// </summary>
        /// <param name="applicationUser">
        /// The user to create a <see cref="ClaimsIdentity"/> for.
        /// </param>
        /// <returns>
        /// A <see cref="ClaimsIdentity"/> representing <paramref name="applicationUser"/>.
        /// </returns>
        public static ClaimsIdentity Create(RegistrationData applicationUser)
        {
            return new ClaimsIdentity(new Claim[]
            {
                // TODO: Claims
            });
        }
    }
}
