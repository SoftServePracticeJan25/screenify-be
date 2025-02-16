using AutoMapper;
using Domain.DTOs.Data.SessionDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
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
    public class SessionService(MovieDbContext context, IMapper mapper) : ISessionService
    {
        public async Task<IEnumerable<SessionDto>> GetAllAsync(SessionQueryObject query)
        {
            var sessions = context.Sessions
            .Include(s => s.Movie)
            .ThenInclude(m => m!.MovieGenres) 
            .Include(s => s.Room)
            .AsQueryable();

            if (query.StartDate.HasValue)
            {
                sessions = sessions.Where(s => s.StartTime.Date >= query.StartDate.Value.Date);
            }

            if (query.EndDate.HasValue)
            {
                sessions = sessions.Where(s => s.StartTime.Date <= query.EndDate.Value.Date);
            }

            if (query.StartTime.HasValue)
            {
                sessions = sessions.Where(s => s.StartTime.TimeOfDay >= query.StartTime.Value);
            }

            if (query.EndTime.HasValue)
            {
                sessions = sessions.Where(s => s.StartTime.TimeOfDay <= query.EndTime.Value);
            }

            if (query.GenreId.HasValue)
            {
                sessions = sessions.Where(s => s.Movie != null &&
                    s.Movie.MovieGenres.Any(mg => mg.GenreId == query.GenreId.Value));
            }

            if (query.MovieId.HasValue)
            {
                sessions = sessions.Where(s => s.MovieId!.Value == query.MovieId.Value);
            }

            var sessionList = await sessions.ToListAsync();
            return mapper.Map<List<SessionDto>>(sessionList);
        }

        public async Task<SessionDto> GetByIdAsync(int id)
        {
            var session = await context.Sessions
                 .Include(s => s.Room) // RoomName
                 .FirstOrDefaultAsync(s => s.Id == id);
            return mapper.Map<SessionDto>(session);
        }

        public async Task<SessionDto> CreateAsync(SessionCreateDto sessionDto)
        {
            var session = mapper.Map<Session>(sessionDto);
            context.Sessions.Add(session);
            await context.SaveChangesAsync();

           
            var createdSession = await context.Sessions
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == session.Id);

            return mapper.Map<SessionDto>(createdSession);
        }


        public async Task<SessionDto> UpdateAsync(int id, SessionCreateDto sessionDto)
        {
            var session = await context.Sessions
                .Include(s => s.Room) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null) throw new KeyNotFoundException("Session not found");

            mapper.Map(sessionDto, session);
            await context.SaveChangesAsync();

            return mapper.Map<SessionDto>(session);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var session = await context.Sessions.FindAsync(id);
            if (session == null) return false;

            context.Sessions.Remove(session);
            await context.SaveChangesAsync();
            return true;
        }
    }
}