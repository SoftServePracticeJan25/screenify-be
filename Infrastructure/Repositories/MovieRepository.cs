using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DbContext _context;

        public MovieRepository(DbContext context)
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

        Task<List<Movie>> IMovieRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}
