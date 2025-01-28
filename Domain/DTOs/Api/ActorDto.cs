using Domain.DTOs.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Api
{
    public class ActorDto
    {
        public string Name { get; set; } 
        public string Bio { get; set; } 
        public DateTime BirthDate { get; set; } 
        public string PhotoUrl { get; set; }
    }
}
