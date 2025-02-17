using AutoMapper;
using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class TicketService(MovieDbContext context, IMapper mapper) : ITicketService
    {
        public async Task<TicketReadDto> AddAsync(Ticket ticket)
        {
            await context.Tickets.AddAsync(ticket);
            await context.SaveChangesAsync();

            var newTicket = await context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(transaction => transaction!.AppUser)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Room)
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            if (newTicket == null)
                throw new Exception("Failed to retrieve the created ticket");

            return mapper.Map<TicketReadDto>(newTicket);
        }


        public async Task<TicketReadDto?> DeleteAsync(int id)
        {
            var ticketModel = await context.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if (ticketModel == null)
            {
                return null;
            }

            context.Tickets.Remove(ticketModel);
            await context.SaveChangesAsync();
            return mapper.Map<TicketReadDto>(ticketModel);
        }

        public async Task<List<TicketReadDto>> GetAllAsync(TicketQueryObject query)
        {
            var ticketsQuery = context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(transaction => transaction!.AppUser)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Room)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.UserId))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Transaction != null && 
                                                    t.Transaction.AppUser != null && 
                                                    t.Transaction.AppUser.Id == query.UserId);
            }

            if (query.MovieId.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.Session != null && 
                                                    t.Session.Movie != null && 
                                                    t.Session.Movie.Id == query.MovieId.Value);
            }

            var tickets = await ticketsQuery.ToListAsync();
            return tickets.Select(t => mapper.Map<TicketReadDto>(t)).ToList();
        }


        public async Task<TicketReadDto?> GetByIdAsync(int id)
        {
            var ticket = await context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(transaction => transaction!.AppUser)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Room)
                .FirstOrDefaultAsync(t => t.Id == id);

            var ticketDto = mapper.Map<TicketReadDto>(ticket);

            return ticketDto;
        }

        public async Task<List<TicketReadDto>> GetUserTicketsAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid token. No user ID found.");

            var tickets = await context.Tickets
                .Include(t => t.Transaction)
                    .ThenInclude(u => u!.AppUser)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Room)
                .Where(t => t.Transaction != null && t.Transaction.AppUserId == userId)
                .ToListAsync();

            return mapper.Map<List<TicketReadDto>>(tickets);
        }



        public async Task<bool> TicketExist(int id)
        {
            return await context.Tickets.AnyAsync(s => s.Id == id);
        }

        public async Task<TicketReadDto?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto)
        {
            var existingTicket = await context.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if(existingTicket == null)
            {
                return null;
            }

            existingTicket.SeatNum = ticketUpdateDto.SeatNum;
            existingTicket.SessionId = ticketUpdateDto.SessionId;
            existingTicket.TransactionId = ticketUpdateDto.TransactionId;

            await context.SaveChangesAsync();

            return mapper.Map<TicketReadDto>(existingTicket);
        }
    }
}