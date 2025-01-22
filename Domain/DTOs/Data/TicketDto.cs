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

        public string? AppUserId { get; set; } // must be string type
        public int? SessionId { get; set; }
    }
}
