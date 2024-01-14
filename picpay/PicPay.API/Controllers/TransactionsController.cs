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
    public void GetTransactionById(Guid id)
    {
        
    }

    [HttpPost]
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
            Value = request.Value,
            PayerId = request.PayerId,
            PayeeId = request.PayeeId,
            CreatedAt = DateTime.Now
        };

        var debitEntry = new Entry()
        {
            Value = -request.Value,
            CreatedAt = DateTime.Now
        };
        
        var creditEntry = new Entry()
        {
            Value = request.Value,
            CreatedAt = DateTime.Now
        };
        
        transaction.Entries.Add(debitEntry);
        transaction.Entries.Add(creditEntry);

        payer.Wallet.Balance -= request.Value;
        payee.Wallet.Balance += request.Value;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
    }
}