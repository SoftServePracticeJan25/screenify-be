using Domain.DTOs.Api;
using Domain.DTOs.Data;
using Domain.DTOs.MovieDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
namespace Domain.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieReadDto>> GetAllAsync(MovieQueryObject query);

        Task<MovieReadDto?> GetByIdAsync(int id);

        Task<MovieReadDto> AddAsync(MovieCreateDto movieCreateDto);

        Task UpdateAsync(int id, MovieCreateDto movieCreateDto);

        Task<MovieReadDto> PatchAsync(int id, MovieUpdateDto movieUpdateDto);

        Task<IEnumerable<MovieReadDto>> GetRecommendedMoviesForUser(string userId);

        Task<bool> DeleteAsync(int id);
    }
}
