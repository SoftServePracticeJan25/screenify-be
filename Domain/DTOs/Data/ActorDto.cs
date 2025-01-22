using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class ActorDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string? PhotoUrl { get; set; } = string.Empty;
    }
}
