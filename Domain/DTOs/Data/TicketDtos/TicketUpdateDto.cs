namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketUpdateDto
    {
        public required int SeatNum { get; init; }
        public required int TransactionId { get; init; }
        public required int SessionId { get; init; }
    }
}