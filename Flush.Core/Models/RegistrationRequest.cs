namespace Flush.Shared.Models
{
    /// <summary>
    /// A request to register a new account.
    /// </summary>
    public class RegistrationRequest
    {
        public string Name { get; set; }
        public string UsernameOrEmail { get; set; }
        public string Room { get; set; }
    }
}
