using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Helpers.QueryObject;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController(IReviewService reviewService, IMapper mapper, UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] ReviewQueryObject queryObject)
        {
            var reviews = await reviewService.GetAllAsync(queryObject);
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var review = await reviewService.GetByIdAsync(id);
            return review == null ? NotFound() : Ok(review);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto reviewCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = userManager.GetUserId(User);
            reviewCreateDto.AppUserId = userId;

            if (await reviewService.ReviewExist(reviewCreateDto.MovieId, reviewCreateDto.AppUserId))
            {
                return BadRequest("Review on this movie by this user already exists");
            }

            var reviewModel = mapper.Map<Review>(reviewCreateDto);
            var reviewReadDto = await reviewService.AddAsync(reviewModel);

            return CreatedAtAction(nameof(GetById), new { id = reviewReadDto.Id }, reviewReadDto);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await reviewService.GetByIdAsync(id);
            if (review == null) return NotFound();

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            
            if (review.AppUserId != userId && !isAdmin)
            {
                return Forbid(); // 403 Forbidden
            }

            var reviewDto = await reviewService.UpdateAsync(id, reviewUpdateDto);
            return Ok(reviewDto);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] ReviewPatchDto reviewPatchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await reviewService.GetByIdAsync(id);
            if (review == null) return NotFound();

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");
   
            if (review.AppUserId != userId && !isAdmin)
            {
                return Forbid(); // 403 Forbidden
            }

            var updatedReview = await reviewService.PatchAsync(id, reviewPatchDto);
            return updatedReview == null ? NotFound() : Ok(updatedReview);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await reviewService.GetByIdAsync(id);
            if (review == null) return NotFound();

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            
            if (review.AppUserId != userId && !isAdmin)
            {
                return Forbid(); // 403 Forbidden
            }

            await reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}
