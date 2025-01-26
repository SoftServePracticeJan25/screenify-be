using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IActorService
    {
        Task<List<Actor>> GetAllAsync();
        Task<Actor?> GetByIdAsync(int id);
        Task<Actor> AddAsync(Actor actor);
        Task<Actor?> UpdateAsync(int id, ActorUpdateDto actorUpdateDto);
        Task <Actor?> DeleteAsync(int id);
        Task<bool> ActorExist(int id);
    }
}