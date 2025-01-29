using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.RoomDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost] 
        public async Task<IActionResult> Add([FromBody] RoomCreateDto roomCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRoom = await _roomService.AddAsync(roomCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> Update(int id, [FromBody] RoomCreateDto roomCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _roomService.UpdateAsync(id, roomCreateDto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
