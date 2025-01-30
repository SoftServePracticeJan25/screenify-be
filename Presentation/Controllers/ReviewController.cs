using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Extentions;
using Domain.Helpers.QueryObject;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController(IReviewService reviewService, IMapper mapper, UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] ReviewQueryObject queryObject)
        {
            var reviews = await reviewService.GetAllAsync(queryObject);

            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ReviewReadDto? review = await reviewService.GetByIdAsync(id);

            if(review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto reviewCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(await reviewService.ReviewExist(reviewCreateDto.MovieId, reviewCreateDto.AppUserId))
            {
                return BadRequest("Review on this movie by this user already exist");
            }

            var reviewModel = mapper.Map<Review>(reviewCreateDto);
            
            var reviewReadDto = await reviewService.AddAsync(reviewModel);

            return CreatedAtAction(nameof(GetById), new { id = reviewReadDto.Id }, reviewReadDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDto = await reviewService.UpdateAsync(id, reviewUpdateDto);

            if(reviewDto == null)
            {
                return NotFound();
            }

            return Ok(reviewDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewModel = await reviewService.DeleteAsync(id);

            if(reviewModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}