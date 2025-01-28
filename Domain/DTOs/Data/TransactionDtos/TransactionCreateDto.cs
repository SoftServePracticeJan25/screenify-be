namespace Domain.DTOs.Data.Transactiondtos
{
    public class TransactionCreateDto
    {
        public required decimal Sum { get; init; }
        public required DateTime CreationTime { get; init; }
        public required string AppUserId { get; init; }
    }
}