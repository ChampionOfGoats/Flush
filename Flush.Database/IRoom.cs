using System;

namespace Flush.Database
{
    public interface IRoom
    {
        string RoomUniqueId { get; }
        string OwnerUniqueId { get; }
        string Name { get; }
    }
}