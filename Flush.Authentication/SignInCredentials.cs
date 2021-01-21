namespace Flush.Authentication
{
    public class SignInCredentials
    {
        public string Username { get; set; }
        public string Room { get; set; }

        // For now, pre-generated.
        public string Password => @"P@ssw0rd*";
    }
}
