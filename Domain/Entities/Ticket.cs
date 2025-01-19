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

        public int? UserId { get; set; }
        //public User? User { get; set; } first solve Identity problem

        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
