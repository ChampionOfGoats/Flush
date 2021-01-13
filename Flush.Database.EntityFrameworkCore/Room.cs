using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flush.Database.EntityFrameworkCore
{
    internal class Room : IRoom
    {
        public int RoomId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Owner { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
