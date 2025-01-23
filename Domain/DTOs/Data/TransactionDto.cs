namespace Domain.DTOs.Data
{
    public class TransactionDto
    {
        public required decimal Sum { get; init; }
        public required DateTime CreationTime { get; init; }
        public required string AppUserId { get; init; }
    }
}
