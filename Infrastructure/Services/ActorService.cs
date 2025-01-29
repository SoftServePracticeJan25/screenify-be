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
using AutoMapper;
namespace Services
{
    public class ActorService : IActorService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;
        public ActorService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActorReadDto> AddAsync(Actor actor)
        {
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
            return _mapper.Map<ActorReadDto>(actor);
        }

        public async Task<ActorReadDto?> DeleteAsync(int id)
        {
            var actorModel = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actorModel == null)
            {
                return null;
            }

            _context.Actors.Remove(actorModel);
            await _context.SaveChangesAsync();
            return _mapper.Map<ActorReadDto>(actorModel);
        }

        public async Task<List<ActorReadDto>> GetAllAsync()
        {
            var actors = await _context.Actors.ToListAsync();
            var actorDtos = actors.Select(x => _mapper.Map<ActorReadDto>(x)).ToList();

            return actorDtos;
        }

        public async Task<ActorReadDto?> GetByIdAsync(int id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            var actorDto = _mapper.Map<ActorReadDto>(actor);

            return actorDto;
        }

        public async Task<ActorReadDto?> UpdateAsync(int id, ActorUpdateDto actorUpdateDto)
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

            return _mapper.Map<ActorReadDto>(existingActor);
        }

        public async Task<bool> ActorExist(int id)
        {
            return await _context.Actors.AnyAsync(s => s.Id == id);
        }
    }
}