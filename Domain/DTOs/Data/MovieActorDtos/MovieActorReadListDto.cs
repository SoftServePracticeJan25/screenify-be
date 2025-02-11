using Domain.DTOs.Data.ActorRoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.MovieActorDtos
{
    public class MovieActorReadListDto
    {
        public ActorDto? Actor { get; set; }
        public ActorRoleDto? RoleName { get; set; }
        public string? CharacterName { get; set; }
    }
}
