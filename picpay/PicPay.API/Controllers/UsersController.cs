using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Entities;
using PicPay.API.Helpers;
using PicPay.API.Models;
using PicPay.API.Models.Request;
using PicPay.API.Models.Response;

namespace PicPay.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateUserRequestModel> _validator;
    private const decimal InitialBalance = 10_000M;

    public UsersController(AppDbContext context, IValidator<CreateUserRequestModel> validator)
    {
        _context = context;
        _validator = validator;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _context.Users.Include(u => u.Wallet)
            .AsNoTracking().ToListAsync();

        var response = users.Select(user => new UserDTO(
            user.Id, 
            user.FullName, 
            user.DocumentNumber, 
            user.Email,
            user.IsSeller,
            user.Wallet.Id));
        
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();
        
        var response = new UserDTO(user.Id, user.FullName, user.DocumentNumber, user.Email, user.IsSeller,
            user.Wallet.Id);
        
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequestModel request)
    {
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

            var user = new User
            {
                FullName = request.FullName,
                DocumentNumber = request.DocumentNumber,
                Email = request.Email,
                Password = request.Password,
                IsSeller = StringHelper.RemoveSpecialCharactersAndLetters(request.DocumentNumber).Length == 14
            };

            var wallet = new Wallet { Balance = InitialBalance };
            user.Wallet = wallet;
        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new CreateUserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                DocumentNumber = user.DocumentNumber,
                Email = user.Email,
                IsSeller = user.IsSeller,
                Wallet = user.Wallet
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
        }
        catch (Exception e)
        {
            var error = new Error(e.Message);
            var errorResponse = new ErrorResponse { StatusCode = 400 };
            errorResponse.Errors.Add(error);
            return BadRequest(errorResponse);
        }
    }
}