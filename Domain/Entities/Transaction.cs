
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Sum { get; set; }
        public DateTime CreationTime { get; set; }
        public string? AppUserId { get; set; } // must be string type
        public AppUser? AppUser { get; set; }

        public List<Ticket> Tickets { get; set; } = [];
    }
}
