using Domain.DTOs.Api;
using Domain.DTOs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomReadDto>> GetAllAsync();
        Task<RoomReadDto?> GetByIdAsync(int id);
        Task<RoomReadDto> AddAsync(RoomCreateDto roomCreateDto);
        Task<bool> UpdateAsync(int id, RoomCreateDto roomCreateDto); 
        Task<bool> DeleteAsync(int id);
    }
}
