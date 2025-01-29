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
        Task<List<ActorRoleReadDto>> GetAllAsync();
        Task<ActorRoleReadDto?> GetByIdAsync(int id);
        Task<ActorRoleReadDto> AddAsync(ActorRole actorRole);
        Task<ActorRoleReadDto?> UpdateAsync(int id, ActorRoleUpdateDto actorRoleUpdateDto);
        Task<ActorRoleReadDto?> DeleteAsync(int id);
        Task<bool> ActorRoleExist(int id);
    }
}