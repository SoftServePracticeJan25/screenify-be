using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace Services
{
    public class ActorService : IActorService
    {
        private readonly MovieDbContext _context;
        public ActorService(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<Actor> AddAsync(Actor actor)
        {
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        public async Task<Actor?> DeleteAsync(int id)
        {
            var actorModel = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actorModel != null)
            {
                return null;
            }

            _context.Actors.Remove(actorModel);
            await _context.SaveChangesAsync();
            return actorModel;
        }

        public async Task<List<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Actor?> GetByIdAsync(int id)
        {
            return await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Actor?> UpdateAsync(int id, ActorUpdateDto actorUpdateDto)
        {
            var existingActor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(existingActor == null)
            {
                return null;
            }

            existingActor.Name = actorUpdateDto.Name;
            existingActor.Bio = actorUpdateDto.Bio;
            existingActor.BirthDate = actorUpdateDto.BirthDate;
            existingActor.PhotoUrl = actorUpdateDto.PhotoUrl;

            await _context.SaveChangesAsync();

            return existingActor;
        }

        public async Task<bool> ActorExist(int id)
        {
            return await _context.Actors.AnyAsync(s => s.Id == id);
        }
    }
}