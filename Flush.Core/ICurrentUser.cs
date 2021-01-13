namespace Flush.Core
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
        string Email { get; }
        string UserName { get; }
        string CurrentRoom { get; }
    }
}
