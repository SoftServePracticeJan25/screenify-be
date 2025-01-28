using Domain.DTOs.Data.Transactiondtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(int id, TransactionUpdateDto transactionUpdateDto);
        Task <Transaction?> DeleteAsync(int id);
        Task<bool> TransactionExist(int id);
    }
}