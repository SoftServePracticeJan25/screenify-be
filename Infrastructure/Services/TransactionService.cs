using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data;
using Domain.DTOs.Data.Transactiondtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly MovieDbContext _context;
        public TransactionService(MovieDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> DeleteAsync(int id)
        {
            var transactionModel = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (transactionModel == null)
            {
                return null;
            }

            _context.Transactions.Remove(transactionModel);
            await _context.SaveChangesAsync();
            return transactionModel;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> TransactionExist(int id)
        {
            return await _context.Transactions.AnyAsync(s => s.Id == id);
        }

        public async Task<Transaction?> UpdateAsync(int id, TransactionUpdateDto transactionUpdateDto)
        {
            var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if(existingTransaction == null)
            {
                return null;
            }

            existingTransaction.Sum = transactionUpdateDto.Sum;
            existingTransaction.CreationTime = transactionUpdateDto.CreationTime;

            await _context.SaveChangesAsync();

            return existingTransaction;
        }
    }
}