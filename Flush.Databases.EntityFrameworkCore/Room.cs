using System.Collections.Generic;

namespace Flush.Databases.EntityFrameworkCore
{
    public class Room : IRoom
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int? Owner { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
