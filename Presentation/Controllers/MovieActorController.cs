using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/movie-actor")]
    public class MovieActorController(IMovieActorService movieActorService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var actors = await movieActorService.GetAllAsync();

            return Ok(actors);
        }

        [HttpGet("{movieId:int}/{actorId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int movieId, [FromRoute] int actorId)
        {
            var movieActor = await movieActorService.GetByIdAsync(movieId, actorId);

            if (movieActor == null)
            {
                return NotFound();
            }

            return Ok(movieActor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieActorCreateDto movieActorCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieActorModel = mapper.Map<MovieActor>(movieActorCreateDto);

            await movieActorService.AddAsync(movieActorModel);

            var movieActorDto = mapper.Map<MovieActorReadDto>(movieActorModel);

            return CreatedAtAction(nameof(GetById), new { movieId = movieActorDto.MovieId, actorId = movieActorDto.ActorId }, movieActorDto);
        }

        [HttpPut("{movieId:int}/{actorId:int}")]
        public async Task<IActionResult> Update([FromRoute] int movieId, [FromRoute] int actorId, [FromBody] MovieActorUpdateDto movieActorUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieActorModel = await movieActorService.UpdateAsync(movieId, actorId, movieActorUpdateDto);

            if (movieActorModel == null)
            {
                return NotFound();
            }

            return Ok(movieActorModel);
        }


        [HttpDelete("{movieId:int}/{actorId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int movieId, [FromRoute] int actorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieActorModel = await movieActorService.DeleteAsync(movieId, actorId);

            if (movieActorModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}