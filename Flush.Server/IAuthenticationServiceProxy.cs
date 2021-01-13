using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flush.Server
{
    /// <summary>
    /// Interface describing an authentication service.
    /// </summary>
    public interface IAuthenticationServiceProxy
    {
        /// <summary>
        /// Get an existing user account.
        /// </summary>
        /// <returns>A matching user, else null.</returns>
        Task<IdentityUser> GetExistingUser();

        /// <summary>
        /// Register a new, permanent user.
        /// </summary>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        Task<IActionResult> RegisterNewUser();

        /// <summary>
        /// Register a guest user.
        /// </summary>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        Task<IActionResult> RegisterGuestUser();

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <returns>
        /// Http Ok, containing a bearer token, on success; else Http BadRequest.
        /// </returns>
        Task<IActionResult> SignIn();

        /// <summary>
        /// Sign a user out.
        /// </summary>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        Task<IActionResult> SignOut();

        /// <summary>
        /// Remove a user.
        /// </summary>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        Task<IActionResult> RemoveUser();
    }
}
