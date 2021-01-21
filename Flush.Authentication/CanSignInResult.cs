using System.ComponentModel;

namespace Flush.Authentication
{
    /// <summary>
    /// Enum describing results of a CanSignIn operation.
    /// </summary>
    public enum CanSignInResult
    {
        /// <summary>
        /// The user can sign in.
        /// </summary>
        [Description("The specifed user may sign in.")]
        Success,

        /// <summary>
        /// The user is already signed in, and cannot sign in again.
        /// </summary>
        [Description("The specified user is already signed in.")]
        SignedIn,

        /// <summary>
        /// The user is locked out.
        /// </summary>
        [Description("The specified user has been locked out.")]
        LockedOut,

        /// <summary>
        /// The user was not found.
        /// </summary>
        [Description("The specified user was not found.")]
        NotFound
    }
}
