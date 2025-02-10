using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.Transactiondtos;
using Domain.DTOs.Data.TransactionDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController(ITransactionService transactionService, IMapper mapper, UserManager<AppUser> userManager, IFilesGenerationService filesGenerationService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await transactionService.GetAllAsync();

            return Ok(transactions);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            TransactionReadDto? transaction = await transactionService.GetByIdAsync(id);

            var userId = userManager.GetUserId(User);
            var IsAdmin = User.IsInRole("Admin");

            if (transaction.AppUserId != userId && IsAdmin) return Forbid();

            if(transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] TransactionCreateDto transactionCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = mapper.Map<Transaction>(transactionCreateDto);
            var userId = userManager.GetUserId(User);

            await transactionService.AddAsync(transaction);

            transaction.AppUserId = userId;

            var transactionDto = mapper.Map<TransactionReadDto>(transaction);

            return CreatedAtAction(nameof(GetById), new { id = transactionDto.Id }, transactionDto);
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TransactionUpdateDto transactionUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionDto = await transactionService.UpdateAsync(id, transactionUpdateDto);

            if(transactionDto == null)
            {
                return NotFound();
            }

            return Ok(transactionDto);
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionModel = await transactionService.DeleteAsync(id);

            if(transactionModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}