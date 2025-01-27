using AutoMapper;
using Domain.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public GenreService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        public async Task<GenreDto?> GetByIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<GenreDto> AddAsync(GenreCreateDto genreCreateDto)
        {       
            var genre = _mapper.Map<Genre>(genreCreateDto);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<bool> UpdateAsync(int id, GenreDto genreDto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            _mapper.Map(genreDto, genre);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
