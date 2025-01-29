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
    public class TransactionService : ITransactionService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;
        public TransactionService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TransactionReadDto> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionReadDto>(transaction);
        }

        public async Task<TransactionReadDto?> DeleteAsync(int id)
        {
            var transactionModel = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (transactionModel == null)
            {
                return null;
            }

            _context.Transactions.Remove(transactionModel);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionReadDto>(transactionModel);
        }

        public async Task<List<TransactionReadDto>> GetAllAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();
            var transactionDtos = transactions.Select(x => _mapper.Map<TransactionReadDto>(x)).ToList();

            return transactionDtos;
        }

        public async Task<TransactionReadDto?> GetByIdAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            var transactionDto = _mapper.Map<TransactionReadDto>(transaction);

            return transactionDto;
        }

        public async Task<bool> TransactionExist(int id)
        {
            return await _context.Transactions.AnyAsync(s => s.Id == id);
        }

        public async Task<TransactionReadDto?> UpdateAsync(int id, TransactionUpdateDto transactionUpdateDto)
        {
            var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if(existingTransaction == null)
            {
                return null;
            }

            existingTransaction.Sum = transactionUpdateDto.Sum;
            existingTransaction.CreationTime = transactionUpdateDto.CreationTime;

            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionReadDto>(existingTransaction);
        }
    }
}