namespace Domain.DTOs.Data.SessionDtos
{
    public class SessionDto
    {
        public required DateTime StartTime { get; set; }
        public required decimal Price { get; set; }

        public required int MovieId { get; set; }
        public required int RoomId { get; set; }
    }
}
