using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Entities;
using PicPay.API.Models.Request;
using PicPay.API.Models.Response;

namespace PicPay.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateUserRequestModel> _validator;
    private const decimal INITIAL_BALANCE = 10_000M;

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
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> CreateUser(CreateUserRequestModel request)
    {
        try
        {
            var result = _validator.ValidateAsync(request);
            if (!result.Result.IsValid)
            {
                return UnprocessableEntity();
            }

            var user = new User
            {
                FullName = request.FullName,
                DocumentNumber = request.DocumentNumber,
                Email = request.Email,
                Password = request.Password
            };

            var wallet = new Wallet { Balance = INITIAL_BALANCE };
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
            return BadRequest(new ErrorResponse { Message = e.Message });
        }
    }
}