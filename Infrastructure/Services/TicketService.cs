using AutoMapper;
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
        private readonly IMapper _mapper;
        public TicketService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TicketReadDto> AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return _mapper.Map<TicketReadDto>(ticket);
        }

        public async Task<TicketReadDto?> DeleteAsync(int id)
        {
            var ticketModel = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if (ticketModel == null)
            {
                return null;
            }

            _context.Tickets.Remove(ticketModel);
            await _context.SaveChangesAsync();
            return _mapper.Map<TicketReadDto>(ticketModel);
        }

        public async Task<List<TicketReadDto>> GetAllAsync()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(transaction => transaction.AppUser) // Добавляем AppUser для получения userId
                .Include(t => t.Session)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s.Room).ToListAsync();
            var ticketDtos = tickets.Select(x => _mapper.Map<TicketReadDto>(x)).ToList();

            return ticketDtos;
        }

        public async Task<TicketReadDto?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(transaction => transaction.AppUser) // Добавляем AppUser для получения userId
                .Include(t => t.Session)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s.Room)
                .FirstOrDefaultAsync(t => t.Id == id);

            var ticketDto = _mapper.Map<TicketReadDto>(ticket);

            return ticketDto;
        }

        public async Task<bool> TicketExist(int id)
        {
            return await _context.Tickets.AnyAsync(s => s.Id == id);
        }

        public async Task<TicketReadDto?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto)
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

            return _mapper.Map<TicketReadDto>(existingTicket);
        }
    }
}