namespace Domain.DTOs.Data.RoomDtos
{
    public class RoomDto
    {
        public required string Name { get; init; }
        public required int SeatsAmount { get; init; }
        public required string CinemaTypeName { get; init; }
    }
}
