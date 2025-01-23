using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class TicketDto
    {
        public int SeatNum { get; set; }

        public int TransactionId { get; set; }
        public int? SessionId { get; set; }
    }
}
