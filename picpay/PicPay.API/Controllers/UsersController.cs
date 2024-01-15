using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Models;
using PicPay.API.Models.RequestModels;
using PicPay.API.Models.ResponseModels;

namespace PicPay.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private const decimal INITIAL_BALANCE = 10_000M;

    public UsersController(AppDbContext context)
    {
        _context = context;
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
    public async Task<ActionResult<User>> GetUserById(int id)
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
    public async Task<ActionResult> CreateUser(CreateUserRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            var user = new User
            {
                FullName = model.FullName,
                DocumentNumber = model.DocumentNumber,
                Email = model.Email,
                Password = model.Password
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