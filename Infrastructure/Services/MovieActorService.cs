using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class MovieActorService : IMovieActorService
    {
        private readonly MovieDbContext _context;
        public MovieActorService(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<MovieActor> AddAsync(MovieActor movieActor)
        {
            await _context.MovieActors.AddAsync(movieActor);
            await _context.SaveChangesAsync();
            return movieActor;
        }

        public async Task<MovieActor?> DeleteAsync(int movieId, int actorId)
        {
            var movieActorModel = await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);

            if (movieActorModel == null)
            {
                return null;
            }

            _context.MovieActors.Remove(movieActorModel);
            await _context.SaveChangesAsync();
            return movieActorModel;
        }

        public async Task<List<MovieActor>> GetAllAsync()
        {
            return await _context.MovieActors.ToListAsync();
        }

        public async Task<MovieActor?> GetByIdAsync(int movieId, int actorId)
        {
            return await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);
        }

        public async Task<bool> MovieActorExist(int movieId, int actorId)
        {
            return await _context.MovieActors.AnyAsync(x => x.MovieId == movieId && x.ActorId == actorId);;
        }

        public async Task<MovieActor?> UpdateAsync(int movieId, int actorId, MovieActorUpdateDto reviewUpdateDto)
        {
            var existingMovieActor = await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);

            if(existingMovieActor == null)
            {
                return null;
            }

            existingMovieActor.CharacterName = reviewUpdateDto.CharacterName;
            existingMovieActor.ActorRoleId = reviewUpdateDto.ActorRoleId;

            await _context.SaveChangesAsync();

            return existingMovieActor;
        }
    }
}