using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketNotifDto
    {
        public required string Title { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string Adress { get; init; }
    }
}