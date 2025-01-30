using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IActorService
    {
        Task<List<ActorReadDto>> GetAllAsync();
        Task<ActorReadDto?> GetByIdAsync(int id);
        Task<ActorReadDto> AddAsync(Actor actor);
        Task<ActorReadDto?> UpdateAsync(int id, ActorUpdateDto actorUpdateDto);
        Task <ActorReadDto?> DeleteAsync(int id);
        Task<bool> ActorExist(int id);
    }
}