using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.Transactiondtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController(ITransactionService transactionService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await transactionService.GetAllAsync();

            return Ok(transactions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Transaction? transaction = await transactionService.GetByIdAsync(id);

            if(transaction == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TransactionDto>(transaction));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionCreateDto transactionCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = mapper.Map<Transaction>(transactionCreateDto);

            await transactionService.AddAsync(transaction);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TransactionUpdateDto transactionUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionModel = await transactionService.UpdateAsync(id, transactionUpdateDto);

            if(transactionModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TransactionDto>(transactionModel));
        }
        [HttpDelete]
        [Route("{id:int}")]
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