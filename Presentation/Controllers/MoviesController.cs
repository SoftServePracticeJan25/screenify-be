using Domain.DTOs.Api;
using Domain.DTOs.Data;
using Domain.DTOs.MovieDtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies); // MovieReadDto
        }

        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie); 
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] MovieCreateDto movieCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMovie = await _movieService.AddAsync(movieCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieCreateDto movieCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.UpdateAsync(id, movieCreateDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] MovieUpdateDto movieUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var updatedMovie = await _movieService.PatchAsync(id, movieUpdateDto);
            return Ok(updatedMovie);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.DeleteAsync(id);
            return NoContent();
        }
    }
}
