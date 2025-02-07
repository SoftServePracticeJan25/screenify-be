using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/actor-role")]
    public class ActorRoleController(IActorRoleService actorRoleService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var actorRoles = await actorRoleService.GetAllAsync();

            return Ok(actorRoles);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ActorRoleReadDto? actorRole = await actorRoleService.GetByIdAsync(id);

            if (actorRole == null)
            {
                return NotFound();
            }

            return base.Ok(actorRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ActorRoleCreateDto actorRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorRole = mapper.Map<ActorRole>(actorRoleDto);

            await actorRoleService.AddAsync(actorRole);

            var actorRoleReadDto = mapper.Map<ActorRoleReadDto>(actorRole);

            return CreatedAtAction(nameof(GetById), new { id = actorRoleReadDto.Id }, actorRoleReadDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ActorRoleUpdateDto actorRoleUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorRoleModel = await actorRoleService.UpdateAsync(id, actorRoleUpdateDto);

            if (actorRoleModel == null)
            {
                return NotFound();
            }

            return base.Ok(actorRoleModel);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorRoleModel = await actorRoleService.DeleteAsync(id);

            if (actorRoleModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
