
using Domain.DTOs.Data.ActorRoleDtos;

namespace Domain.DTOs.Data.MovieActorDtos
{
    public class MovieActorDto
    {
        public ActorDto Actor { get; set; }
        public ActorRoleDto RoleName { get; set; }
        public required int MovieId { get; init; }
        public required int ActorId { get; init; }

        public required string CharacterName { get; init; }
    }
}
