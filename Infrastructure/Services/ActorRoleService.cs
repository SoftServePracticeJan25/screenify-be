using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Services
{
    public class ActorRoleService : IActorRoleService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public ActorRoleService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActorRoleReadDto> AddAsync(ActorRole actorRole)
        {
            await _context.ActorRoles.AddAsync(actorRole);
            await _context.SaveChangesAsync();

            return _mapper.Map<ActorRoleReadDto>(actorRole);
        }

        public async Task<ActorRoleReadDto?> DeleteAsync(int id)
        {
            var actorRole = await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (actorRole == null)
            {
                return null;
            }

            _context.ActorRoles.Remove(actorRole);
            await _context.SaveChangesAsync();
            return _mapper.Map<ActorRoleReadDto>(actorRole);
        }

        public async Task<List<ActorRoleReadDto>> GetAllAsync()
        {
            var actorRoles = await _context.ActorRoles.ToListAsync();
            var actorRoleDtos = actorRoles.Select(x => _mapper.Map<ActorRoleReadDto>(x)).ToList();

            return actorRoleDtos;
        }

        public async Task<ActorRoleReadDto?> GetByIdAsync(int id)
        {
            var actorRole = await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ActorRoleReadDto>(actorRole);
        }

        public async Task<ActorRoleReadDto?> UpdateAsync(int id, ActorRoleUpdateDto actorRoleUpdateDto)
        {
            var existingActorRole = await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (existingActorRole == null)
            {
                return null;
            }

            existingActorRole.RoleName = actorRoleUpdateDto.RoleName;

            await _context.SaveChangesAsync();

            return _mapper.Map<ActorRoleReadDto>(existingActorRole);
        }

        public async Task<bool> ActorRoleExist(int id)
        {
            return await _context.ActorRoles.AnyAsync(s => s.Id == id);
        }
    }
}
