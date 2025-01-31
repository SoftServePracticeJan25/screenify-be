using Domain.DTOs.Data.CinemaTypeDtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/cinematype")]
    public class CinemaTypeController : ControllerBase
    {
        private readonly ICinemaTypeService _cinemaTypeService;

        public CinemaTypeController(ICinemaTypeService cinemaTypeService)
        {
            _cinemaTypeService = cinemaTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cinematypes = await _cinemaTypeService.GetAllAsync();
            return Ok(cinematypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cinematypes = await _cinemaTypeService.GetByIdAsync(id);
            if (cinematypes == null) { return NotFound(); }
            return Ok(cinematypes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CinemaTypeDto cinemaTypeDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var createdCinemaType = await _cinemaTypeService.AddAsync(cinemaTypeDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCinemaType.Id }, createdCinemaType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CinemaTypeDto cinemaTypeDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var updated = await _cinemaTypeService.UpdateAsync(id, cinemaTypeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _cinemaTypeService.DeleteAsync(id);
            if (!deleted) { return NotFound(); }

            return NoContent();
        }
    }
}
