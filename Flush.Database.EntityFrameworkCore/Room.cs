using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Room : IRoom
    {
        [Key] public int RoomId { get; set; }

        [Required] public string RoomUniqueId { get; set; }

        [NotMapped] public string OwnerUniqueId => UniqueUser.ApplicationUserId;

        [Required] public string Name { get; set; }

        // One room has Many sessions.
        public ICollection<Session> Sessions { get; set; }

        // One room may have One unique user.
        public int? UniqueUserId { get; set; }
        public UniqueUser UniqueUser { get; set; }
    }
}
