using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.RoomDtos
{
    public class RoomCreateDto
    {
        public required string Name { get; set; }
        public int SeatsAmount { get; set; }
        public int? CinemaTypeId { get; set; }
    }
}
