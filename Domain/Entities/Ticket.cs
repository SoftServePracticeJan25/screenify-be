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

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
