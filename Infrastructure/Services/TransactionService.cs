using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.Transactiondtos;
using Domain.DTOs.Data.TransactionDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TransactionService(MovieDbContext context, IMapper mapper) : ITransactionService
    {
        public async Task<TransactionReadDto> AddAsync(Transaction transaction)
        {
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return mapper.Map<TransactionReadDto>(transaction);
        }

        public async Task<TransactionReadDto?> DeleteAsync(int id)
        {
            var transactionModel = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (transactionModel == null)
            {
                return null;
            }

            context.Transactions.Remove(transactionModel);
            await context.SaveChangesAsync();
            return mapper.Map<TransactionReadDto>(transactionModel);
        }

        public async Task<List<TransactionReadDto>> GetAllAsync()
        {
            var transactions = await context.Transactions.ToListAsync();
            var transactionDtos = transactions.Select(x => mapper.Map<TransactionReadDto>(x)).ToList();

            return transactionDtos;
        }

        public async Task<TransactionReadDto?> GetByIdAsync(int id)
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            var transactionDto = mapper.Map<TransactionReadDto>(transaction);

            return transactionDto;
        }

        public async Task<bool> TransactionExist(int id)
        {
            return await context.Transactions.AnyAsync(s => s.Id == id);
        }

        public async Task<TransactionReadDto?> UpdateAsync(int id, TransactionUpdateDto transactionUpdateDto)
        {
            var existingTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if(existingTransaction == null)
            {
                return null;
            }

            existingTransaction.Sum = transactionUpdateDto.Sum;
            existingTransaction.CreationTime = transactionUpdateDto.CreationTime;

            await context.SaveChangesAsync();

            return mapper.Map<TransactionReadDto>(existingTransaction);
        }
    }
}