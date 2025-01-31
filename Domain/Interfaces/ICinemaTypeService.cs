using Domain.DTOs.Api;
using Domain.DTOs.Data.CinemaTypeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICinemaTypeService
    {
        Task<IEnumerable<CinemaTypeReadDto>> GetAllAsync();

        Task<CinemaTypeReadDto?> GetByIdAsync(int id);

        Task<CinemaTypeReadDto> AddAsync(CinemaTypeDto cinemaTypeDto);

        Task<CinemaTypeReadDto> UpdateAsync(int id, CinemaTypeDto cinemaTypeDto);

        Task<bool> DeleteAsync(int id);
    }
}
