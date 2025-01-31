

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatNum { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
