namespace Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Bio { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string? PhotoUrl { get; set; } = string.Empty;

        public List<MovieActor> MovieActors { get; set; } = [];
    }
}
