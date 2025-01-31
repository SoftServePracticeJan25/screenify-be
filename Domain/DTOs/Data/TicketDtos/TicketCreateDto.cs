namespace Domain.DTOs.Data.TicketDtos
{
    public class TicketCreateDto
    {
        public required int SeatNum { get; init; }
        public required int TransactionId { get; init; }
        public required int SessionId { get; init; }
    }
}