using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiServerBackend.Data;
using SkiServerBackend.Models;
using System.Security.Cryptography;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _context.Mitarbeiter.FirstOrDefaultAsync(m => m.Email == model.Email);
        if (user == null || !VerifyPassword(model.Passwort, user.PasswortHash))
        {
            return Unauthorized(new { message = "Ungültige Login-Daten" });
        }

        return Ok(new { message = "Login erfolgreich!", userId = user.MitarbeiterID });
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        using var sha256 = SHA256.Create();
        var hashedInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return hashedInput == storedHash;
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Passwort { get; set; }
}
