namespace Domain.DTOs.Data.SessionDtos
{
    public class SessionDto
    {
        public string Id { get; set; }
        public required DateTime StartTime { get; set; }
        public required decimal Price { get; set; }

        public required int MovieId { get; set; }
        public required int RoomId { get; set; }
        public required string RoomName { get; set; }
    }
}
