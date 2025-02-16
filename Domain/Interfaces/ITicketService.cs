using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
using System.Security.Claims;
namespace Domain.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketReadDto>> GetAllAsync(TicketQueryObject query);
        Task<TicketReadDto?> GetByIdAsync(int id);
        Task<List<TicketReadDto>> GetUserTicketsAsync(ClaimsPrincipal user);
        Task<TicketReadDto> AddAsync(Ticket ticket);
        Task<TicketReadDto?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto);
        Task <TicketReadDto?> DeleteAsync(int id);
        Task<bool> TicketExist(int id);
    }
}