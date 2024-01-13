using Microsoft.AspNetCore.Identity;
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
    public async Task<ActionResult> CreateUser(CreateUserRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity();
        }

        User user = new()
        {
            FullName = model.FullName,
            DocumentNumber = model.DocumentNumber,
            Email = model.Email,
            Password = model.Password
        };

        Wallet wallet = new();
        user.Wallet = wallet;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        CreateUserResponseModel response = new CreateUserResponseModel()
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
}