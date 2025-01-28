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
        Task<List<MovieActor>> GetAllAsync();
        Task<MovieActor?> GetByIdAsync(int movieId, int actorId);
        Task<MovieActor> AddAsync(MovieActor movieActor);
        Task<MovieActor?> UpdateAsync(int movieId, int actorId, MovieActorUpdateDto reviewUpdateDto);
        Task <MovieActor?> DeleteAsync(int movieId, int actorId);
        Task<bool> MovieActorExist(int movieId, int actorId);
    }
}