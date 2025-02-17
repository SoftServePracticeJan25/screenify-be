using AutoMapper;
using Domain.DTOs.Data.TicketDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
using Domain.Interfaces;
using Hangfire;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] TicketQueryObject query)
        {
            var tickets = await ticketService.GetAllAsync(query);
            return Ok(tickets);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var ticket = await ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await transactionService.GetByIdAsync(ticket.TransactionId);
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

        [HttpGet("my-tickets")]
        [Authorize]
        public async Task<IActionResult> GetUserTickets()
        {
            try
            {
                var userTickets = await ticketService.GetUserTicketsAsync(User);
                return Ok(userTickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] TicketCreateDto ticketCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = userManager.GetUserId(User);
            var transactionDto = await transactionService.GetByIdAsync(ticketCreateDto.TransactionId);

            if (transactionDto == null || transactionDto.AppUserId != userId)
            {
                return Forbid();
            }

            bool isAvailable = await ticketService.IsSeatAvailable(ticketCreateDto.SessionId, ticketCreateDto.SeatNum);

            if (!isAvailable)
            {
                return BadRequest(new { message = "This seat is already taken for the selected session." });
            }

            var ticket = mapper.Map<Ticket>(ticketCreateDto);
            var ticketDto = await ticketService.AddAsync(ticket);

            // Sending email to user with needed files
            var user = await userManager.FindByIdAsync(userId);

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User email not found.");
            }
            BackgroundJob.Enqueue<ISendGridEmailService>(emailService =>
                 emailService.SendTransactionEventTicketEmail(
                     new Ticket { Id = ticket.Id },
                     new Transaction
                     {
                         Id = transactionDto.Id,
                         Sum = transactionDto.Sum,
                         CreationTime = transactionDto.CreationTime
                     },
                         user.Email
                        ));


            //if (!user.EmailConfirmed) return BadRequest("Confirm email first");

            return CreatedAtAction(nameof(GetById), new { id = ticketDto.Id }, ticketDto);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "User,Admin")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TicketUpdateDto ticketUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await transactionService.GetByIdAsync(ticket.TransactionId);
            if (transaction == null || (transaction.AppUserId != userId && !isAdmin))
            {
                return Forbid();
            }

            var updatedTicket = await ticketService.UpdateAsync(id, ticketUpdateDto);
            return Ok(updatedTicket);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var transaction = await transactionService.GetByIdAsync(ticket.TransactionId);
            if (transaction == null || (transaction.AppUserId != userId && !isAdmin))
            {
                return Forbid();
            }

            await ticketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
