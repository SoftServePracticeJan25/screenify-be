using AutoMapper;
using Domain.DTOs.Data.SessionDtos;
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
    public class SessionService : ISessionService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public SessionService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionDto>> GetAllAsync()
        {
            var sessions = await _context.Sessions.ToListAsync();
            return _mapper.Map<IEnumerable<SessionDto>>(sessions);
        }

        public async Task<SessionDto> GetByIdAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            return _mapper.Map<SessionDto>(session);
        }

        public async Task<SessionDto> CreateAsync(SessionDto sessionDto)
        {
            var session = _mapper.Map<Session>(sessionDto);
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return _mapper.Map<SessionDto>(session);
        }

        public async Task<SessionDto> UpdateAsync(int id, SessionDto sessionDto)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) throw new KeyNotFoundException("Session not found");

            _mapper.Map(sessionDto, session); // Обновляет свойства `session` из `sessionDto`
            await _context.SaveChangesAsync();

            return _mapper.Map<SessionDto>(session);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}