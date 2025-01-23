
namespace Domain.DTOs.Data
{
    public class RoomDto
    {
        public required string Name { get; init; }
        public required int SeatsAmount { get; init; }
        public required int CinemaTypeId { get; init; }
    }
}
