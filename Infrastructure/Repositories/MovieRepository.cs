using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Set<Movie>().ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Set<Movie>().FindAsync(id);
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Set<Movie>().AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Set<Movie>().Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);
            if (movie != null)
            {
                _context.Set<Movie>().Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

    }
}
