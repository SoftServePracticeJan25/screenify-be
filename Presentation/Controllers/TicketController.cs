using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController(ITicketService ticketService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await ticketService.GetAllAsync();

            return Ok(tickets);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Ticket? ticket = await ticketService.GetByIdAsync(id);

            if(ticket == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TicketDto>(ticket));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketCreateDto ticketCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = mapper.Map<Ticket>(ticketCreateDto);

            await ticketService.AddAsync(ticket);

            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TicketUpdateDto ticketUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticketModel = await ticketService.UpdateAsync(id, ticketUpdateDto);

            if(ticketModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TicketDto>(ticketModel));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticketModel = await ticketService.DeleteAsync(id);

            if(ticketModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}