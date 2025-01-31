namespace Domain.DTOs.Data.ActorDtos
{
    public class ActorUpdateDto
    {
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public DateTime BirthDate { get; init; }
        public required string PhotoUrl { get; init; }
    }
}