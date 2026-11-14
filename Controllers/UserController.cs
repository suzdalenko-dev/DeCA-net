using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DeCaDbContext _context;
     public UserController(DeCaDbContext context)
    {
        _context = context;
    }

    // http://localhost:5006/
    [HttpGet("/")]
    public async Task<IActionResult> Index(){
        byte edad = 44;
        short temperatura = 25;
        int cantidad = 1500;
        long poblacion = 8000000000L;

        float altura = 1.80f;
        double peso = 72.5;
        decimal precio = 125.50m;

        char inicial = 'A';
        bool activo = true;
        string nombre = "Alexey";

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id > 0)
            .OrderBy(u => u.Id)
            .Select(u => new {u.Id, u.Email, u.CreatedAt, u.LastLoginAt })
            .ToListAsync();
        
        bool connected = await _context.Database.CanConnectAsync();
        
        return Ok(new { 
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            info = "Alexey Suzdalenko",
            users = users,
            connected = connected
        });
    }

    // http://localhost:5006/api/user/login?email=alexey@gmail.com&password=123456
    [HttpGet("login")]
    public IActionResult Login(string email, string password){

        return Ok(new {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            info = "Login successful",
            x = email,
            y = password
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();
        var user     = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user != null) {
           return Ok(new { message = "Usuario encontrado", id = user.Id, email= user.Email });
        } else {
            user = new User {
                Email = email,
                Password = request.Password,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                LoginNumber = 1
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new {
                message = "Usuario creado",
                id = user.Id,
                email = user.Email,
                createdAt = user.CreatedAt
            });
        }
    }
}