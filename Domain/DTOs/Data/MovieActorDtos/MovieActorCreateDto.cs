using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.MovieActorDtos
{
    public class MovieActorCreateDto
    {
        public required int MovieId { get; init; }
        public required int ActorId { get; init; }
        public required int ActorRoleId { get; init; }
        public required string CharacterName { get; init; }
    }
}