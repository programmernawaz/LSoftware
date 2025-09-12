using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Entities;
using PathLabAPI.Utilities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly PathLabDbContext _context;
        public AuthController(PathLabDbContext context)
        {
            _context = context; 
        }

        public class LoginRequest 
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = ""; 
        
        }
        public class RegisterRequest
        { 
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string? DisplayName { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req) 
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            {
                return BadRequest("Username and Password is required!");
            }

            var hash= HashHelper.Sha256(req.Password);

            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == req.Username && u.PasswordHash == hash);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { user.Id, user.Username,user.DisplayName });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req) 
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            {
                return BadRequest("Username and Password is required!");
            }

            if (await _context.AppUsers.AnyAsync(u => u.Username == req.Username))
            {
                return Conflict("username already exists.");
            }

            var user = new AppUser 
            {
                Username = req.Username,
                DisplayName = req.DisplayName,
                PasswordHash = HashHelper.Sha256(req.Password)
            };
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id, user.Username, user.DisplayName });

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var u = await _context.AppUsers.FindAsync(id);
            if (u == null) return NotFound();
            return Ok(new { u.Id, u.Username, u.DisplayName });
        }
    }
}
