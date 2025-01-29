using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Api
{
    public class MovieActorCreateDto
    {
        public int ActorId { get; set; } 
        public int RoleId { get; set; } 
        public string CharacterName { get; set; } 
    }
}
