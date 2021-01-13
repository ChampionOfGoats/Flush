using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Flush
{
    /// <summary>
    /// Factory for the creation of new Claims Identities
    /// </summary>
    public static class ClaimsIdentityFactory
    {
        /// <summary>
        /// Create a <see cref="ClaimsIdentity"/> from the given <see cref="IdentityUser"/>.
        /// </summary>
        /// <param name="identityUser">
        /// The user to create a <see cref="ClaimsIdentity"/> for.
        /// </param>
        /// <returns>
        /// A <see cref="ClaimsIdentity"/> representing <paramref name="identityUser"/>.
        /// </returns>
        public static ClaimsIdentity Create(IdentityUser identityUser)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                new Claim(ClaimTypes.GivenName, "FirstName"),
                new Claim(ClaimTypes.Surname, "LastName"),
                new Claim(ClaimTypes.Email, identityUser.Email),
                new Claim(ClaimTypes.Name, identityUser.UserName),
                new Claim(ClaimTypes.UserData, "Room Name")
            });
        }
    }
}
