namespace Flush.Contracts
{
    public interface ICurrentUser
    {
        string DisplayName { get; }
        string UniqueId { get; }
        string RoomUniqueId { get; }
    }
}
