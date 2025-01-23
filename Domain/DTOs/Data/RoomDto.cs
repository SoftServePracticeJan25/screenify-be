using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class RoomDto
    {
        public string Name { get; set; }
        public int SeatsAmount { get; set; }
        public int? CinemaTypeId { get; set; }
    }
}
