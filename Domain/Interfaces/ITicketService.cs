using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto);
        Task <Ticket?> DeleteAsync(int id);
        Task<bool> TicketExist(int id);
    }
}