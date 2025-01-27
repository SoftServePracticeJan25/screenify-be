using Domain.DTOs.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto?> GetByIdAsync(int id);
        Task<GenreDto> AddAsync(GenreCreateDto genreCreateDto); 
        Task<bool> UpdateAsync(int id, GenreDto genreDto);
        Task<bool> DeleteAsync(int id);
    }
}
