namespace Flush.Authentication
{
    public class RegistrationData
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        // For now, pre-generated.
        public string Password => @"P@ssw0rd*";
    }
}
