using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(4,2)")]
        public decimal Price { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
