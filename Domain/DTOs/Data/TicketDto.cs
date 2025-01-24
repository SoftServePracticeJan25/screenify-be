

namespace Domain.DTOs.Data
{
    public class TicketDto
    {
        public required int SeatNum { get; init; }

        public required int TransactionId { get; init; }
        public required int SessionId { get; init; }
    }
}
