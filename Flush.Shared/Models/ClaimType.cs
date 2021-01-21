using System.ComponentModel;

namespace Flush.Models
{
    /// <summary>
    /// Flush-specific claim types for use with <see cref="System.Security.Claims.ClaimsPrincipal"/>.
    /// </summary>
    public enum ClaimType
    {
        /// <summary>
        /// The forename.
        /// </summary>
        [Description(@"com:flush:firstname")]
        FirstName,

        /// <summary>
        /// The surname.
        /// </summary>
        [Description(@"com:flush:lastname")]
        LastName,

        /// <summary>
        /// The middle names.
        /// </summary>
        [Description(@"com:flush:middlenames")]
        MiddleNames,

        /// <summary>
        /// The full name (FirstName + LastName).
        /// </summary>
        [Description(@"com:flush:fullname")]
        FullName,

        /// <summary>
        /// The room unique identifier.
        /// </summary>
        [Description(@"com:flush:roomid")]
        RoomUniqueId,

        /// <summary>
        /// The email address.
        /// </summary>
        [Description(@"com:flush:email")]
        EmailAddress,

        /// <summary>
        /// The user name.
        /// </summary>
        [Description(@"com:flush:username")]
        UserName,

        /// <summary>
        /// The user identifier.
        /// </summary>
        [Description(@"com:flush:userid")]
        UserIdentifier,
    }
}
