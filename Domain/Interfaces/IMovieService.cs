using Domain.DTOs.Api;
using Domain.DTOs.Data;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieReadDto>> GetAllAsync();
   
        Task<MovieReadDto?> GetByIdAsync(int id);

        Task<MovieReadDto> AddAsync(MovieCreateDto movieCreateDto);

        Task UpdateAsync(int id, MovieCreateDto movieCreateDto);

        Task<bool> DeleteAsync(int id);
    }


}
