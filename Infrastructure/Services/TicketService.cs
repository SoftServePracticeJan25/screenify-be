using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly MovieDbContext _context;
        public TicketService(MovieDbContext context)
        {
            _context = context;
        }
        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> DeleteAsync(int id)
        {
            var ticketModel = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if (ticketModel == null)
            {
                return null;
            }

            _context.Tickets.Remove(ticketModel);
            await _context.SaveChangesAsync();
            return ticketModel;
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> TicketExist(int id)
        {
            return await _context.Tickets.AnyAsync(s => s.Id == id);
        }

        public async Task<Ticket?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto)
        {
            var existingTicket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if(existingTicket == null)
            {
                return null;
            }

            existingTicket.SeatNum = ticketUpdateDto.SeatNum;
            existingTicket.SessionId = ticketUpdateDto.SessionId;
            existingTicket.TransactionId = ticketUpdateDto.TransactionId;

            await _context.SaveChangesAsync();

            return existingTicket;
        }
    }
}