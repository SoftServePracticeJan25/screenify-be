using Domain.Interfaces;
using Infrastructure.DataAccess;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.DTOs.Data.RoomDtos;

namespace Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public RoomService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomReadDto>> GetAllAsync()
        {
            var rooms = await _context.Rooms.Include(r => r.CinemaType).ToListAsync();
            return _mapper.Map<IEnumerable<RoomReadDto>>(rooms);
        }

        public async Task<RoomReadDto?> GetByIdAsync(int id)
        {
            var room = await _context.Rooms.Include(r => r.CinemaType).FirstOrDefaultAsync(r => r.Id == id);
            return _mapper.Map<RoomReadDto>(room);
        }

        public async Task<RoomReadDto> AddAsync(RoomCreateDto roomCreateDto)
        {
            var room = _mapper.Map<Room>(roomCreateDto);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoomReadDto>(room);
        }

        public async Task<bool> UpdateAsync(int id, RoomCreateDto roomCreateDto)
        {
            var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRoom == null)
                return false;

            _mapper.Map(roomCreateDto, existingRoom);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

