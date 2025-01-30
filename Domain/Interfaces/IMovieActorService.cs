using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMovieActorService
    {
        Task<List<MovieActorReadDto>> GetAllAsync();
        Task<MovieActorReadDto?> GetByIdAsync(int movieId, int actorId);
        Task<MovieActorReadDto> AddAsync(MovieActor movieActor);
        Task<MovieActorReadDto?> UpdateAsync(int movieId, int actorId, MovieActorUpdateDto reviewUpdateDto);
        Task <MovieActorReadDto?> DeleteAsync(int movieId, int actorId);
        Task<bool> MovieActorExist(int movieId, int actorId);
    }
}