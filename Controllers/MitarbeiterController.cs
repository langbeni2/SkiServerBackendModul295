using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkiServerBackend.Data;
using SkiServerBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class MitarbeiterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public MitarbeiterController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // 🔹 1️⃣ Mitarbeiter-Registrierung (Passwort wird gehasht)
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] MitarbeiterRegister request)
    {
        if (await _context.Mitarbeiter.AnyAsync(m => m.Email == request.Email))
        {
            return BadRequest(new { message = "E-Mail bereits registriert." });
        }

        var mitarbeiter = new Mitarbeiter
        {
            Name = request.Name,
            Email = request.Email,
            PasswortHash = BCrypt.Net.BCrypt.HashPassword(request.Passwort)
        };

        _context.Mitarbeiter.Add(mitarbeiter);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registrierung erfolgreich", mitarbeiterId = mitarbeiter.MitarbeiterID });
    }

    // 🔹 2️⃣ Mitarbeiter-Login (JWT-Token generieren)
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var mitarbeiter = await _context.Mitarbeiter
            .FirstOrDefaultAsync(m => m.Email == request.Email);

        if (mitarbeiter == null || !BCrypt.Net.BCrypt.Verify(request.Passwort, mitarbeiter.PasswortHash))
        {
            return Unauthorized(new { message = "Falsche E-Mail oder Passwort." });
        }

        var token = GenerateJwtToken(mitarbeiter);

        return Ok(new
        {
            message = "Login erfolgreich",
            token,
            mitarbeiterId = mitarbeiter.MitarbeiterID,
            name = mitarbeiter.Name
        });
    }

    // 🔹 3️⃣ JWT-Token generieren
    private string GenerateJwtToken(Mitarbeiter mitarbeiter)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("JWT Key fehlt!"));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, mitarbeiter.MitarbeiterID.ToString()),
                new Claim(ClaimTypes.Name, mitarbeiter.Name),
                new Claim(ClaimTypes.Email, mitarbeiter.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

// 📌 Datenmodelle für API-Anfragen
public class LoginRequest
{
    public string Email { get; set; }
    public string Passwort { get; set; }
}

public class MitarbeiterRegister
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Passwort { get; set; }
}
