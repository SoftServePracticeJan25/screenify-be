using Domain.Interfaces;
using Infrastructure.DataAccess;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Data.CinemaTypeDtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CinemaTypeService : ICinemaTypeService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public CinemaTypeService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CinemaTypeReadDto>> GetAllAsync()
        {
            var cinemaTypes = await _context.CinemaTypes.ToListAsync();
            return _mapper.Map<IEnumerable<CinemaTypeReadDto>>(cinemaTypes);
        }

        public async Task<CinemaTypeReadDto?> GetByIdAsync(int id)
        {
            var cinemaType = await _context.CinemaTypes.FindAsync(id);
            if (cinemaType == null)
                return null;

            return _mapper.Map<CinemaTypeReadDto>(cinemaType);
        }

        public async Task<CinemaTypeReadDto> AddAsync(CinemaTypeDto cinemaTypeDto)
        {
            var cinemaType = _mapper.Map<CinemaType>(cinemaTypeDto);
            _context.CinemaTypes.Add(cinemaType);
            await _context.SaveChangesAsync();

            return _mapper.Map<CinemaTypeReadDto>(cinemaType);
        }

        public async Task<CinemaTypeReadDto> UpdateAsync(int id, CinemaTypeDto cinemaTypeDto)
        {
            var existingCinemaType = await _context.CinemaTypes.FindAsync(id);
            if (existingCinemaType == null)
                throw new KeyNotFoundException($"CinemaType with ID {id} not found.");

            _mapper.Map(cinemaTypeDto, existingCinemaType);
            await _context.SaveChangesAsync();

            return _mapper.Map<CinemaTypeReadDto>(existingCinemaType); 
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cinemaType = await _context.CinemaTypes.FindAsync(id);
            if (cinemaType == null)
                return false;

            _context.CinemaTypes.Remove(cinemaType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

