
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public List<Ticket> Tickets { get; set; } = [];
    }
}
