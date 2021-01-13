namespace Flush.Database
{
    public interface IRoom
    {
        int RoomId { get; }
        string Name { get; }
        string Owner { get; }
    }
}