using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketReadDto
    {
        public required int Id { get; set; }
        public required int SeatNum { get; init; }
        public required int TransactionId { get; init; }
        public required int SessionId { get; init; }
    }
}