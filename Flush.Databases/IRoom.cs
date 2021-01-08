namespace Flush.Databases
{
    public interface IRoom
    {
        int RoomId { get; }
        string Name { get; }
        int? Owner { get; }
    }
}