using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketReadDto
    {
        public int Id { get; set; }
        public int SeatNum { get; set; }

        public int TransactionId { get; set; }
        public string? UserId { get; set; }
        public int? SessionId { get; set; }
        public DateTime TransactionTime { get; set; }

        public int? MovieId { get; set; }
        public string? Title { get; set; }
        public int Duration { get; set; }
        public string? PosterUrl { get; set; }

        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }

        public string? RoomName { get; set; }
    }

}