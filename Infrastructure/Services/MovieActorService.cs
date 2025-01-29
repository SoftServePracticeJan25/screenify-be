using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Services
{
    public class MovieActorService : IMovieActorService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;
        public MovieActorService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MovieActorReadDto> AddAsync(MovieActor movieActor)
        {
            await _context.MovieActors.AddAsync(movieActor);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieActorReadDto>(movieActor);
        }

        public async Task<MovieActorReadDto?> DeleteAsync(int movieId, int actorId)
        {
            var movieActorModel = await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);

            if (movieActorModel == null)
            {
                return null;
            }

            _context.MovieActors.Remove(movieActorModel);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieActorReadDto>(movieActorModel);
        }

        public async Task<List<MovieActorReadDto>> GetAllAsync()
        {
            var movieActors = await _context.MovieActors.ToListAsync();
            var movieActorDtos = movieActors.Select(x => _mapper.Map<MovieActorReadDto>(x)).ToList();

            return movieActorDtos;
        }

        public async Task<MovieActorReadDto?> GetByIdAsync(int movieId, int actorId)
        {
            var movieActor = await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);
            var movieActorDto = _mapper.Map<MovieActorReadDto>(movieActor);

            return movieActorDto;
        }

        public async Task<bool> MovieActorExist(int movieId, int actorId)
        {
            return await _context.MovieActors.AnyAsync(x => x.MovieId == movieId && x.ActorId == actorId);;
        }

        public async Task<MovieActorReadDto?> UpdateAsync(int movieId, int actorId, MovieActorUpdateDto reviewUpdateDto)
        {
            var existingMovieActor = await _context.MovieActors.FirstOrDefaultAsync(x => x.MovieId == movieId && x.ActorId == actorId);

            if(existingMovieActor == null)
            {
                return null;
            }

            existingMovieActor.CharacterName = reviewUpdateDto.CharacterName;
            existingMovieActor.ActorRoleId = reviewUpdateDto.ActorRoleId;

            await _context.SaveChangesAsync();

            return _mapper.Map<MovieActorReadDto>(existingMovieActor);
        }
    }
}