using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DeCaDbContext _context;
    private readonly EmailSender _emailSender;
    public UserController(DeCaDbContext context, EmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    // http://localhost:5006/
    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
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
            .Select(u => new { u.Id, u.Email, u.CreatedAt, u.LastLoginAt })
            .ToListAsync();

        bool connected = await _context.Database.CanConnectAsync();

        return Ok(new
        {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            info = "Alexey Suzdalenko",
            users = users,
            connected = connected
        });
    }

    // http://localhost:5006/api/user/login?email=alexey@gmail.com
    [HttpGet("login")]
    public IActionResult Loginget(string email)
    {

        return Ok(new
        {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            info = "Login successful",
            x = email
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string email)
    {
        email = email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        byte[] bytes = RandomNumberGenerator.GetBytes(32);

        if (user == null)
        {
            user = new User
            {
                Email = email,
                CreatedAt = DateTime.Now,
                LastLoginAt = DateTime.Now,
                LoginNumber = 1
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        if (user != null)
        {
            string token = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
            string link = $"https://documento-deca-net.web.app/verify#token={token}";

            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            byte[] hashBytes = SHA256.HashData(tokenBytes);
            string tokenHash = Convert.ToBase64String(hashBytes);

            Console.WriteLine("tokenBytes="+string.Join(", ", tokenBytes));
            Console.WriteLine("hashBytes=" +string.Join(", ", hashBytes));

            user.LoginTokenHash = tokenHash;
            user.LastLoginAt = DateTime.Now;
            user.LoginNumber = user.LoginNumber + 1;
            user.LoginTokenExpiresAt = DateTime.UtcNow.AddDays(11);
            await _context.SaveChangesAsync();
            await _emailSender.SendLoginLinkAsync(email, link);

            return Ok(new { message = "Correo Enviado", id = user.Id, email = user.Email });
        }
        else
        {
            return Ok(new { message = "ERROR", id = 0, email = email });
        }
    }


    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token)) return BadRequest(new { message = "Token no proporcionado" });

        // string token  => bytes array
        byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(request.Token);
        // bytes array  => has bytes array 32 bytes
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        // bytes hash array =>
        string tokenHash = Convert.ToBase64String(hashBytes);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginTokenHash == tokenHash);
        if (user == null) return Unauthorized(new { message = "Token inválido" });

        if (user.LoginTokenExpiresAt == null || user.LoginTokenExpiresAt <= DateTime.Now) {
            return Unauthorized(new { message = "El enlace ha caducado" });
        }

        byte[] bearerBytes = RandomNumberGenerator.GetBytes(32);

        string bearerToken = Convert.ToBase64String(bearerBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");

        byte[] bearerHashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(bearerToken));

        user.LoginTokenHash = null;
        user.LoginTokenExpiresAt = null;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Autenticación correcta",
            id = user.Id,
            email = user.Email,
            accessToken = bearerToken,
        });
    }


}

/*
Task            representa una operación que puede terminar en el futuro.
IActionResult   es una interfaz de ASP.NET Core que representa el resultado de una acción HTTP.
LoginRequest    es una clase que representa los datos que recibes del cliente
*/