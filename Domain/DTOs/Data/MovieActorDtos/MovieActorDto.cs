
using Domain.DTOs.Data.ActorRoleDtos;
using System.Text.Json.Serialization;

namespace Domain.DTOs.Data.MovieActorDtos
{
    public class MovieActorDto
    {
        public int MovieId { get; set; } 
        public int ActorId { get; set; }
        public required int ActorRoleId { get; init; }
        public string CharacterName { get; set; }
    }
}
