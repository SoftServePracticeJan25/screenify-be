﻿using Domain.DTOs.Data.SessionDtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var sessions = await _sessionService.GetByIdAsync(id);
            if (sessions == null)
            {
                return NotFound();
            }
            return Ok(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SessionDto session)
        {
            var createdSession = await _sessionService.CreateAsync(session);
            return CreatedAtAction(nameof(GetById), new { id = createdSession.MovieId }, createdSession);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SessionDto session)
        {
            try
            {
                var updatedSession = await _sessionService.UpdateAsync(id, session);
                return Ok(updatedSession);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sessionService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}