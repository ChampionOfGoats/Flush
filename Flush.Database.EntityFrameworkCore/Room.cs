using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Room : IRoom
    {
        [Key] public int RoomId { get; set; }

        [Required] public string RoomUniqueId { get; set; }

        public string OwnerUniqueId { get; set; }

        [Required] public string Name { get; set; }

        // One-to-Many with Sessions
        public ICollection<Session> Sessions { get; set; }
    }
}
