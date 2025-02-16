using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.RoomDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController(IRoomService roomService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await roomService.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] RoomCreateDto roomCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRoom = await roomService.AddAsync(roomCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomCreateDto roomCreateDto)
        {
            var isUpdated = await roomService.UpdateAsync(id, roomCreateDto);
            if (!isUpdated)
                return NotFound(new { message = "Room not found" });

            var updatedRoom = await roomService.GetByIdAsync(id); 
            return Ok(updatedRoom); 
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await roomService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
