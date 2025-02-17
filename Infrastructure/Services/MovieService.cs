using AutoMapper;
using Domain.DTOs.Api;
using Domain.DTOs.MovieDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public MovieService(MovieDbContext context, IMapper mapper,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieReadDto>> GetAllAsync(MovieQueryObject query)
        {
            var moviesQuery = _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.ActorRole)
                .AsQueryable();

            if (query.GenreId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.MovieGenres.Any(mg => mg.GenreId == query.GenreId.Value));
            }

            var movies = await moviesQuery.ToListAsync();
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
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");             

            return _mapper.Map<MovieReadDto>(movie);
        }

        public async Task<MovieReadDto> AddAsync(MovieCreateDto movieCreateDto)
        {
            if (movieCreateDto.GenreIds == null || !movieCreateDto.GenreIds.Any())
                throw new ArgumentNullException(nameof(movieCreateDto.GenreIds), "GenreIds cannot be null or empty.");

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

            if (savedMovie == null)
                throw new Exception("Failed to retrieve the saved movie.");

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

            if (movieCreateDto.GenreIds == null)
                throw new ArgumentNullException(nameof(movieCreateDto.GenreIds), "GenreIds cannot be null.");


            var movieGenresToRemove = await _context.MovieGenres
                .Where(mg => mg.MovieId == existingMovie.Id)
                .ToListAsync();

            _context.MovieGenres.RemoveRange(movieGenresToRemove);
            await _context.SaveChangesAsync(); 

            existingMovie.MovieGenres = movieCreateDto.GenreIds
                .Select(genreId => new MovieGenre { MovieId = existingMovie.Id, GenreId = genreId })
                .ToList();

            if (movieCreateDto.Actors == null)
                throw new ArgumentNullException(nameof(movieCreateDto.Actors), "Actors list cannot be null.");

            existingMovie.MovieActors = _mapper.Map<List<MovieActor>>(movieCreateDto.Actors);

            await _context.SaveChangesAsync();
        }

        public async Task<MovieReadDto> PatchAsync(int id, MovieUpdateDto movieUpdateDto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            if (!string.IsNullOrEmpty(movieUpdateDto.Title))
                movie.Title = movieUpdateDto.Title;

            if (movieUpdateDto.Duration.HasValue)
                movie.Duration = movieUpdateDto.Duration.Value;

            if (!string.IsNullOrEmpty(movieUpdateDto.PosterUrl))
                movie.PosterUrl = movieUpdateDto.PosterUrl;

            if (movieUpdateDto.GenreIds != null)
            {
                var genres = await _context.Genres
                    .Where(g => movieUpdateDto.GenreIds.Contains(g.Id))
                    .ToListAsync();

                movie.MovieGenres = genres.Select(g => new MovieGenre { GenreId = g.Id }).ToList();
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<MovieReadDto>(movie);
        }

        public async Task<IEnumerable<MovieReadDto>> GetRecommendedMoviesForUser(string userId)
        {
            var purchasedMovies = await _context.Transactions
                .Where(t => t.AppUserId == userId)
                .SelectMany(t => t.Tickets.Select(ticket => ticket.Session!.Movie!))
                .Include(m => m!.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .GroupBy(m => m!.Id) 
                .Select(g => g.First()) 
                .ToListAsync();

            if (purchasedMovies.Count == 0)
                return Enumerable.Empty<MovieReadDto>();

           
            var genreIds = purchasedMovies
                .SelectMany(m => m.MovieGenres.Select(mg => mg.GenreId))
                .Distinct()
                .ToList();

            
            var recommendedMovies = await _context.Movies
                .Where(m => m.MovieGenres.Any(mg => genreIds.Contains(mg.GenreId)) && !purchasedMovies.Contains(m))
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .ToListAsync();

            if (recommendedMovies.Count == 0)
                return Enumerable.Empty<MovieReadDto>();

            
            var sortedMovies = recommendedMovies
                .Select(m => new
                {
                    Movie = m,
                    MatchCount = m.MovieGenres.Count(mg => genreIds.Contains(mg.GenreId))
                })
                .OrderByDescending(m => m.MatchCount)
                .ThenByDescending(m => m.Movie.Id)
                .Take(6)
                .Select(m => m.Movie)
                .ToList();

            return _mapper.Map<IEnumerable<MovieReadDto>>(sortedMovies);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Sessions)
                    .ThenInclude(s => s.Tickets)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return false;

            foreach (var session in movie.Sessions)
            {
                _context.Tickets.RemoveRange(session.Tickets);
            }

            _context.Sessions.RemoveRange(movie.Sessions);
            _context.Reviews.RemoveRange(movie.Reviews);
            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
