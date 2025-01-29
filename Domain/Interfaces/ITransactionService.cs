using Domain.DTOs.Data.Transactiondtos;
using Domain.DTOs.Data.TransactionDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionReadDto>> GetAllAsync();
        Task<TransactionReadDto?> GetByIdAsync(int id);
        Task<TransactionReadDto> AddAsync(Transaction transaction);
        Task<TransactionReadDto?> UpdateAsync(int id, TransactionUpdateDto transactionUpdateDto);
        Task <TransactionReadDto?> DeleteAsync(int id);
        Task<bool> TransactionExist(int id);
    }
}