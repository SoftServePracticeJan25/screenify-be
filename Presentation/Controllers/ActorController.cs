using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/actor")]
    public class ActorController(IActorService actorService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var actors = await actorService.GetAllAsync();

            return Ok(actors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ActorReadDto? actor = await actorService.GetByIdAsync(id);

            if(actor == null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActorCreateDto actorCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actor = mapper.Map<Actor>(actorCreateDto);

            await actorService.AddAsync(actor);

            var actorDto = mapper.Map<ActorReadDto>(actor);

            return CreatedAtAction(nameof(GetById), new { id = actorDto.Id }, actorDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ActorUpdateDto actorUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorModel = await actorService.UpdateAsync(id, actorUpdateDto);

            if(actorModel == null)
            {
                return NotFound();
            }

            return Ok(actorModel);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorModel = await actorService.DeleteAsync(id);

            if(actorModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}