using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatNum { get; set; }

        public string? AppUserId { get; set; } // must be string type
        public AppUser? AppUser { get; set; }

        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
