namespace Domain.DTOs.Data
{
    public class ActorDto
    {
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public DateTime BirthDate { get; init; }
        public required string PhotoUrl { get; init; }
    }
}
