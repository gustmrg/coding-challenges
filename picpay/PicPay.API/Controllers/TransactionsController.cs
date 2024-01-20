using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Exceptions;
using PicPay.API.Entities;
using PicPay.API.Models;
using PicPay.API.Models.Request;
using PicPay.API.Models.Response;
using RestSharp;

namespace PicPay.API.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateTransactionRequestModel> _validator;

    public TransactionsController(AppDbContext context, IValidator<CreateTransactionRequestModel> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
    {
        try
        {
            var transaction = await _context.Transactions
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return NotFound();

            var payer = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == transaction.PayerId);
        
            var payee = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == transaction.PayeeId);

            if (payer == null) throw new NotFoundException($"User not found with id {transaction.PayerId}");
            if (payee == null) throw new NotFoundException($"User not found with id {transaction.PayeeId}");

            var response = new TransactionDTO
            {
                Id = transaction.Id,
                Amount = transaction.Amount.ToString("F", CultureInfo.InvariantCulture),
                Payer = new UserDTO(payer.Id, payer.FullName, payer.Wallet.Id),
                Payee = new UserDTO(payee.Id, payee.FullName, payee.Wallet.Id),
                CreatedAt = transaction.CreatedAt
            };
        
            return Ok(response);
        }
        catch (Exception e)
        {
            var error = new Error(e.Message);
            var errorResponse = new ErrorResponse { StatusCode = 400 };
            errorResponse.Errors.Add(error);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateTransaction(CreateTransactionRequestModel request)
    {
        using var dbTransaction = _context.Database.BeginTransaction();
        
        try
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage));
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 422,
                    Errors = errors.ToList(),
                };
                return UnprocessableEntity(errorResponse);
            }

            var payer = await _context.Users.Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == request.PayerId);
            
            if (payer == null)  throw new NotFoundException($"User not found with id {request.PayerId}");

            if (payer.Wallet.Balance < request.Value) throw new BalanceException("Balance is not enough to complete the transaction");

            if (payer.IsSeller) throw new TransactionException("Sellers cannot send money to other users");
            
            var payee = await _context.Users.Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == request.PayeeId);
            
            if (payee == null) throw new NotFoundException($"User not found with id {request.PayeeId}");

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
                throw new UnauthorizedException("Transaction has not been authorized");
            }
            
            var authorizationResponseData = JsonSerializer.Deserialize<TransactionAuthorizationResponse>(authorizationResponse.Content);

            if (authorizationResponseData?.Message is null || !authorizationResponseData.Message.Equals("Autorizado", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedException("Transaction has not been authorized");
            }
            
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            await SendTransactionNotification();
            
            await dbTransaction.CommitAsync();

            var response = new CreateTransactionResponseModel
            {
                Transaction = new TransactionDTO
                {
                    Id = transaction.Id,
                    Amount = transaction.Amount.ToString("F", CultureInfo.InvariantCulture),
                    Payer = new UserDTO(payer.Id, payer.FullName, payer.Wallet.Id),
                    Payee = new UserDTO(payee.Id, payee.FullName, payee.Wallet.Id),
                    CreatedAt = transaction.CreatedAt,
                    Entries = transaction.Entries.Select(e => 
                        new EntryDTO(
                            e.Id, 
                            e.Amount.ToString("F", CultureInfo.InvariantCulture), 
                            e.TransactionId, 
                            e.WalletId, 
                            e.CreatedAt))
                },
            };
            
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, response);
        }
        catch (Exception e)
        {
            dbTransaction.Rollback();
            var response = new ErrorResponse { StatusCode = 400 };
            response.Errors.Add(new Error(e.Message));
            return BadRequest(response);
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