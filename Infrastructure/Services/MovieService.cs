using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTOs;
using Domain.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Получение всех фильмов
        public async Task<IEnumerable<MovieReadDto>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.ActorRole)
                .ProjectTo<MovieReadDto>(_mapper.ConfigurationProvider) // Маппинг на ReadDto
                .ToListAsync();
        }

        // Получение фильма по ID
        public async Task<MovieReadDto?> GetByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.ActorRole)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MovieReadDto>(movie); // Маппинг на ReadDto
        }

        // Добавление нового фильма
        public async Task AddAsync(MovieCreateDto movieCreateDto)
        {
            var movie = _mapper.Map<Movie>(movieCreateDto); // Маппинг на сущность
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        // Обновление фильма
        public async Task UpdateAsync(int id, MovieCreateDto movieCreateDto)
        {
            var existingMovie = await _context.Movies
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            // Применяем изменения из DTO
            _mapper.Map(movieCreateDto, existingMovie);

            await _context.SaveChangesAsync();
        }

        // Удаление фильма
        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
