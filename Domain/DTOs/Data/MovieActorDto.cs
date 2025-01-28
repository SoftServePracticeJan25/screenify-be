using Domain.DTOs.Api;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class MovieActorDto
    {
        public ActorDto Actor { get; set; }
        public string CharacterName { get; set; } = string.Empty; 
        public string RoleName { get; set; } 
    }
}
