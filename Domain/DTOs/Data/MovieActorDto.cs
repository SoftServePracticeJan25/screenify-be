
namespace Domain.DTOs.Data
{
    public class MovieActorDto
    {
        public required int MovieId { get; init; }
        public required int ActorId { get; init; }
        public required int ActorRoleId { get; init; }
        public required string CharacterName { get; init; }
    }
}
