namespace Domain.Entities
{
    public class CinemaType
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        public List<Room> Rooms { get; set; } = [];
    }
}
