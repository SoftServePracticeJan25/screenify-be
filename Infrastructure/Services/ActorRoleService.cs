using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ActorRoleService : IActorRoleService
    {
        private readonly MovieDbContext _context;

        public ActorRoleService(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<ActorRole> AddAsync(ActorRole actorRole)
        {
            await _context.ActorRoles.AddAsync(actorRole);
            await _context.SaveChangesAsync();
            return actorRole;
        }

        public async Task<ActorRole?> DeleteAsync(int id)
        {
            var actorRole = await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (actorRole == null)
            {
                return null;
            }

            _context.ActorRoles.Remove(actorRole);
            await _context.SaveChangesAsync();
            return actorRole;
        }

        public async Task<List<ActorRole>> GetAllAsync()
        {
            return await _context.ActorRoles.ToListAsync();
        }

        public async Task<ActorRole?> GetByIdAsync(int id)
        {
            return await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ActorRole?> UpdateAsync(int id, ActorRoleUpdateDto actorRoleUpdateDto)
        {
            var existingActorRole = await _context.ActorRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (existingActorRole == null)
            {
                return null;
            }

            existingActorRole.RoleName = actorRoleUpdateDto.RoleName;

            await _context.SaveChangesAsync();

            return existingActorRole;
        }

        public async Task<bool> ActorRoleExist(int id)
        {
            return await _context.ActorRoles.AnyAsync(s => s.Id == id);
        }
    }
}
