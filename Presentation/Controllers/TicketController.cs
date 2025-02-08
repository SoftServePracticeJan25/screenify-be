using System.Text;
using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController(ITicketService ticketService, IMapper mapper, IFilesGenerationService filesGenerationService, MovieDbContext context) : ControllerBase
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
            TicketReadDto? ticket = await ticketService.GetByIdAsync(id);

            if(ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketCreateDto ticketCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = mapper.Map<Ticket>(ticketCreateDto);
            await ticketService.AddAsync(ticket);

            // EVENT FILE
            var ticketWithDetails = await context.Tickets
                .Include(t => t.Session)
                .ThenInclude(s => s.Movie)
                .Include(t => t.Session.Room)
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            if (ticketWithDetails == null)
                return NotFound("Ticket not found after creation.");

            string icsContent = filesGenerationService.GenerateCalendarEvent(ticket);

            return File(Encoding.UTF8.GetBytes(icsContent), "text/calendar", "event.ics");
            
            // TICKET PDF
            // var ticketWithDetails = await context.Tickets
            //     .Include(t => t.Session)
            //     .ThenInclude(s => s.Movie)
            //     .Include(t => t.Session.Room)
            //     .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            // if (ticketWithDetails == null)
            //     return NotFound("Ticket not found after creation.");

            // byte[] pdfBytes = filesGenerationService.GenerateTicketPdf(ticketWithDetails);
            // return File(pdfBytes, "application/pdf", "ticket.pdf");
            
            // DEFAULT
            // var ticketDto = mapper.Map<TicketReadDto>(ticket);

            // return CreatedAtAction(nameof(GetById), new { id = ticketDto.Id }, ticketDto);
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

            return Ok(ticketModel);
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