using AutoMapper;
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

        public async Task<IEnumerable<MovieReadDto>> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.ActorRole)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MovieReadDto>>(movies);
        }

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

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            return _mapper.Map<MovieReadDto>(movie);
        }

        public async Task<MovieReadDto> AddAsync(MovieCreateDto movieCreateDto)
        {
            var movie = _mapper.Map<Movie>(movieCreateDto);

            var genres = await _context.Genres
                .Where(g => movieCreateDto.GenreIds.Contains(g.Id))
                .ToListAsync();
            movie.MovieGenres = genres.Select(g => new MovieGenre { GenreId = g.Id }).ToList();

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var savedMovie = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.ActorRole)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            return _mapper.Map<MovieReadDto>(savedMovie);
        }

        public async Task UpdateAsync(int id, MovieCreateDto movieCreateDto)
        {
            var existingMovie = await _context.Movies
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _mapper.Map(movieCreateDto, existingMovie);

            _context.MovieGenres.RemoveRange(existingMovie.MovieGenres);
            var genres = await _context.Genres
                .Where(g => movieCreateDto.GenreIds.Contains(g.Id))
                .ToListAsync();
            existingMovie.MovieGenres = genres.Select(g => new MovieGenre { GenreId = g.Id }).ToList();

            _context.MovieActors.RemoveRange(existingMovie.MovieActors);
            existingMovie.MovieActors = movieCreateDto.Actors.Select(actor => new MovieActor
            {
                ActorId = actor.ActorId,
                ActorRoleId = actor.ActorRoleId,
                CharacterName = actor.CharacterName
            }).ToList();

            await _context.SaveChangesAsync();
        }
  
        public async Task<bool> DeleteAsync(int id)
        {
            var sessions = await _context.Sessions.Where(s => s.MovieId == id).ToListAsync();
            if (sessions.Any())
                _context.Sessions.RemoveRange(sessions);

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
