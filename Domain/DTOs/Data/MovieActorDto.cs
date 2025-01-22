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
        public int? MovieId { get; set; }
        public int? ActorId { get; set; }
        public int? ActorRoleId { get; set; }
        public string? CharacterName { get; set; } = string.Empty;
    }
}
