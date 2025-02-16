using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketFileDto
    {
        public required int SeatNum { get; init; }
        public required string Title { get; init; }
        public DateTime StartTime { get; init; }
        public decimal Price { get; init; }
        public string? Name { get; init; } // Room
    }
}