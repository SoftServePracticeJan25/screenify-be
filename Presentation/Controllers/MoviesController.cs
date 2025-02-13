using Domain.DTOs.Api;
using Domain.DTOs.Data;
using Domain.DTOs.MovieDtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly MovieDbContext _context;

        public MoviesController(IMovieService movieService, MovieDbContext context)
        {
            _movieService = movieService;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies); // MovieReadDto
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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


        [HttpGet("recommended")]
        [Authorize(Roles =("User,Admin"))]
        public async Task<IActionResult> GetRecommendedMoviesForUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Ok(new List<MovieReadDto>()); // empty []

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Ok(new List<MovieReadDto>()); // empty []

            var recommendedMovies = await _movieService.GetRecommendedMoviesForUser(user);
            return Ok(recommendedMovies);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Sessions)
                    .ThenInclude(s => s.Tickets)
                .Include(m => m.Reviews) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound(new { message = "Movie not found" });
            }

           
            foreach (var session in movie.Sessions)
            {
                _context.Tickets.RemoveRange(session.Tickets);
            }

           
            _context.Sessions.RemoveRange(movie.Sessions);

         
            _context.Reviews.RemoveRange(movie.Reviews);

            
            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
