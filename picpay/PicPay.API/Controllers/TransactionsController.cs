using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Models;
using PicPay.API.Models.RequestModels;

namespace PicPay.API.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public void GetTransactionById(Guid id)
    {
        
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateTransaction(CreateTransactionRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity();
        }

        if (request.Value <= 0)
        {
            return BadRequest();
        }

        var payer = await _context.Users.Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == request.PayerId);
        
        if (payer == null)
        {
            return BadRequest();
        }
        
        var payee = await _context.Users.Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == request.PayeeId);
        
        if (payee == null)
        {
            return BadRequest();
        }

        var transaction = new Transaction()
        {
            Amount = request.Value,
            PayerId = request.PayerId,
            PayeeId = request.PayeeId,
            CreatedAt = DateTime.Now
        };

        var debitEntry = new Entry()
        {
            Amount = -request.Value,
            CreatedAt = DateTime.Now
        };
        
        var creditEntry = new Entry()
        {
            Amount = request.Value,
            CreatedAt = DateTime.Now
        };
        
        transaction.Entries.Add(debitEntry);
        transaction.Entries.Add(creditEntry);

        payer.Wallet.Transactions.Add(transaction);
        payer.Wallet.Entries.Add(debitEntry);
        payer.Wallet.Balance += debitEntry.Amount;
        payee.Wallet.Transactions.Add(transaction);
        payee.Wallet.Entries.Add(creditEntry);
        payee.Wallet.Balance += creditEntry.Amount;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
    }
}