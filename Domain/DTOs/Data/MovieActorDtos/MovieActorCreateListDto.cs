using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.MovieActorDtos
{
    public class MovieActorCreateListDto
    {
        public required int ActorId { get; init; }
        public required int ActorRoleId { get; init; }
        public required string CharacterName { get; init; }
    }
}
