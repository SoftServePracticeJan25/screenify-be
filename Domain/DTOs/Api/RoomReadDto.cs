using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Api
{
    public class RoomReadDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int SeatsAmount { get; set; }
        public int? CinemaTypeId { get; set; }
    }
}
