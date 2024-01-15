using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Exceptions;
using PicPay.API.Models;
using PicPay.API.Models.RequestModels;
using PicPay.API.Models.ResponseModels;
using RestSharp;

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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Entries)
            .FirstOrDefaultAsync(t => t.Id == id);
        return transaction == null ? NotFound() : Ok(transaction);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateTransaction(CreateTransactionRequestModel request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            if (request.Value <= 0)
            {
                throw new TransactionInvalidValueException("EX001 - Transaction amount must be value greater than zero");
            }

            var payer = await _context.Users.Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == request.PayerId);
            
            if (payer == null)
            {
                throw new UserNotFoundException("EX004 - User not found");
            }

            if (payer.Wallet.Balance < request.Value)
            {
                throw new WalletBalanceException("EX002 - Balance is not enough to complete the transaction");
            }
            
            var payee = await _context.Users.Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == request.PayeeId);
            
            if (payee == null)
            {
                throw new UserNotFoundException("EX004 - User not found");
            }

            var transaction = new Transaction
            {
                Amount = request.Value,
                PayerId = request.PayerId,
                PayeeId = request.PayeeId,
                CreatedAt = DateTime.Now
            };

            var debitEntry = new Entry
            {
                Amount = -request.Value,
                CreatedAt = DateTime.Now
            };
            
            var creditEntry = new Entry
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
            
            var authorizationResponse = await GetTransactionAuthorization();

            if (authorizationResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new TransactionUnauthorizedException("EX003 - Transaction was not authorized");
            }
            
            var response = JsonSerializer.Deserialize<TransactionAuthorizationResponse>(authorizationResponse.Content);

            if (response?.Message is null || !response.Message.Equals("Autorizado"))
            {
                throw new TransactionUnauthorizedException("EX003 - Transaction was not authorized");
            }
            
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            await SendTransactionNotification();
            
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse { Message = e.Message });
        }
    }

    private async Task<RestResponse> GetTransactionAuthorization()
    {
        var client = new RestClient("https://run.mocky.io/v3/");
        var request = new RestRequest("5794d450-d2e2-4412-8131-73d0293ac1cc");
        var response = await client.ExecuteGetAsync(request);
        return response;
    }
    
    private async Task SendTransactionNotification()
    {
        var client = new RestClient("https://run.mocky.io/v3/");
        var request = new RestRequest("54dc2cf1-3add-45b5-b5a9-6bf7e7f1f4a6", Method.Post);
        request.AddJsonBody(new { IsCompleted = true });
        var response = await client.ExecutePostAsync<TransactionNotificationResponse>(request);
        Console.WriteLine(response.Data is { Message: true });
    }

    private record TransactionAuthorizationResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; init; } = string.Empty;
    }

    private record TransactionNotificationResponse
    {
        [JsonPropertyName("message")]
        public bool Message { get; init; }
    }
}