using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.DTOs.Data.ActorRoleDtos;

namespace Domain.Interfaces
{
    public interface IActorRoleService
    {
        Task<List<ActorRole>> GetAllAsync();
        Task<ActorRole?> GetByIdAsync(int id);
        Task<ActorRole> AddAsync(ActorRole actorRole);
        Task<ActorRole?> UpdateAsync(int id, ActorRoleUpdateDto actorRoleUpdateDto);
        Task<ActorRole?> DeleteAsync(int id);
        Task<bool> ActorRoleExist(int id);
    }
}