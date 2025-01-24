namespace Domain.Entities
{
    public class ActorRole
    {
        public int Id { get; set; }
        public string? RoleName { get; set; } = string.Empty;

        public List<MovieActor> MovieActors { get; set; } = [];
    }
}
