using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
        public static ClaimsIdentity Create(ApplicationUser applicationUser)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimType.UserIdentifier.Description(), applicationUser.Id),
                new Claim(ClaimType.UserName.Description(), applicationUser.UserName),
                new Claim(ClaimType.FirstName.Description(), applicationUser.FirstName),
                new Claim(ClaimType.LastName.Description(), applicationUser.LastName),
                // TODO: Room should be injected as a role. E.g. an authed rooms role that enables access to the given rooms.
                // The user roles will determine which room they are in.
                // For a guest user, this will only ever be the room they have joined.
                // For registered users, this may be several rooms.
                // We then need to understand the different between owners and players.
                // RoomName_Owner roles?
                // new Claim(ClaimType.RoomName.Description(), applicationUser.RoomName),
                new Claim(ClaimType.EmailAddress.Description(), applicationUser.Email),
            });
        }
    }
}
