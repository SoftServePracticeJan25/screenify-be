using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class SessionDto
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public decimal Price { get; set; }

        public int? MovieId { get; set; }
        public int? RoomId { get; set; }
    }
}
