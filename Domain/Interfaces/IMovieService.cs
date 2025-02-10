using Domain.DTOs.Api;
using Domain.DTOs.Data;
using Domain.DTOs.MovieDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieReadDto>> GetAllAsync();

        Task<MovieReadDto?> GetByIdAsync(int id);

        Task<MovieReadDto> AddAsync(MovieCreateDto movieCreateDto);

        Task UpdateAsync(int id, MovieCreateDto movieCreateDto);

        Task<MovieReadDto> PatchAsync(int id, MovieUpdateDto movieUpdateDto);
        
        Task<IEnumerable<MovieReadDto>> GetRecommendedMovies(int movieId);

        Task<IEnumerable<MovieReadDto>> GetRecommendedMoviesForUser(AppUser user);

        Task<bool> DeleteAsync(int id);
    }
}
