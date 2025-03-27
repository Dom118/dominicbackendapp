using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;
using MinimalX.Data;

namespace MinimalX.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {   
        // connection to database
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // match email
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user == null ? NotFound() : Ok(user);
        }

        // create a user
        [HttpPost]
        public async Task<ActionResult<User>> CreateProduct(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            // handle successful login
            return Ok(user);
        }

    }
}