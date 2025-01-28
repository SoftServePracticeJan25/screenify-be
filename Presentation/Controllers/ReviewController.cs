using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Extentions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController(IReviewService reviewService, IMapper mapper, UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await reviewService.GetAllAsync();

            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Review? review = await reviewService.GetByIdAsync(id);

            if(review == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ReviewDto>(review));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto reviewCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);

            var reviewModel = mapper.Map<Review>(reviewCreateDto);
            reviewModel.AppUserId = appUser.Id;

            await reviewService.AddAsync(reviewModel);

            return CreatedAtAction(nameof(GetById), new { id = reviewModel.Id }, mapper.Map<ReviewReadDto>(reviewModel));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ReviewUpdateDto actorUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewModel = await reviewService.UpdateAsync(id, actorUpdateDto);

            if(reviewModel == null)
            {
                return NotFound();
            }

            var reviewDto = mapper.Map<ReviewReadDto>(reviewModel);

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