using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController(
        ITicketService ticketService,
        IMapper mapper,
        UserManager<AppUser> userManager,
        ITransactionService transactionService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITransactionService _transactionService = transactionService;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await _transactionService.GetByIdAsync(ticket.TransactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.AppUserId != userId && !isAdmin)
            {
                return Forbid(); // If not an Admin or Owner -> 403 Forbidden
            }

            return Ok(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] TicketCreateDto ticketCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userManager.GetUserId(User);
            var transaction = await _transactionService.GetByIdAsync(ticketCreateDto.TransactionId);


            if (transaction == null || transaction.AppUserId != userId)
            {
                return Forbid();
            }

            var ticket = _mapper.Map<Ticket>(ticketCreateDto);
            await _ticketService.AddAsync(ticket);

            var ticketDto = _mapper.Map<TicketReadDto>(ticket);
            return CreatedAtAction(nameof(GetById), new { id = ticketDto.Id }, ticketDto);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TicketUpdateDto ticketUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await _transactionService.GetByIdAsync(ticket.TransactionId);
            if (transaction == null || (transaction.AppUserId != userId && !isAdmin))
            {
                return Forbid(); 
            }

            var updatedTicket = await _ticketService.UpdateAsync(id, ticketUpdateDto);
            return Ok(updatedTicket);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await _transactionService.GetByIdAsync(ticket.TransactionId);
            if (transaction == null || (transaction.AppUserId != userId && !isAdmin))
            {
                return Forbid(); 
            }

            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
