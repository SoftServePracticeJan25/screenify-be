using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.SessionDtos
{
    public class SessionCreateDto
    {
        public string? Id { get; set; }
        public required DateTime StartTime { get; set; }
        public required decimal Price { get; set; }
        public required int MovieId { get; set; }
        public required int RoomId { get; set; } 
    }

}
