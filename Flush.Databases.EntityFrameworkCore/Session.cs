using System;
using System.Collections.Generic;

namespace Flush.Databases.EntityFrameworkCore
{
    public class Session : ISession
    {
        public int SessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
        public ICollection<Trace> Traces { get; set; }
    }
}
