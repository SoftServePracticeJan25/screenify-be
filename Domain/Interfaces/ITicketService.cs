using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketReadDto>> GetAllAsync();
        Task<TicketReadDto?> GetByIdAsync(int id);
        Task<TicketReadDto> AddAsync(Ticket ticket);
        Task<TicketReadDto?> UpdateAsync(int id, TicketUpdateDto ticketUpdateDto);
        Task <TicketReadDto?> DeleteAsync(int id);
        Task<bool> TicketExist(int id);
    }
}