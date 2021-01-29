using System.Threading.Tasks;

namespace Flush.Authentication
{
    /// <summary>
    /// Interface describing an authentication service.
    /// </summary>
    public interface IAuthenticationServiceProxy
    {
        /// <summary>
        /// Get an existing user account.
        /// </summary>
        /// <param name="userName">The email address.</param>
        /// <returns>The ID of the user.</returns>
        Task<UserInfo> GetUserByEmail(string emailAddress, bool ignoreNull = false);

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registrationData">The registration information.</param>
        /// <returns>The ID of the user, or null.</returns>
        Task<UserInfo> RegisterUser(RegistrationData registrationData);

        /// <summary>
        /// Get a value indicating whether the user can sign in.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<CanSignInResult> CanSignIn(UserInfo userInfo);

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <returns>
        /// Http Ok, containing a cookie, on success; else Http BadRequest.
        /// </returns>
        Task<UserInfo> SignIn(SignInCredentials signInCredentials);

        /// <summary>
        /// Sign a user out.
        /// </summary>
        /// <returns>Http Ok on success, else Http BadRequest.</returns>
        Task<bool> SignOut();

        /// <summary>
        /// Remove a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>True on success, else false.</returns>
        Task<bool> RemoveUser(UserInfo userInfo);
    }
}
