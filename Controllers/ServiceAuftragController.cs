using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiServerBackend.Data;
using SkiServerBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ServiceAuftragController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiceAuftragController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Liste aller offenen Serviceaufträge abrufen (KEINE Authentifikation erforderlich)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceAuftrag>>> GetOffeneAuftraege()
    {
        var offeneAuftraege = await _context.ServiceAuftraege
            .Where(a => a.StatusID == 1) // 1 = "Offen"
            .Include(a => a.Kunde) // Kunde-Daten mitladen
            .Include(a => a.Dienstleistung) // Dienstleistung-Daten mitladen
            .ToListAsync();

        return Ok(offeneAuftraege);
    }

    [HttpPost]
    public async Task<IActionResult> ErstelleServiceAuftrag([FromBody] ServiceAuftrag request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Überprüfen, ob die Dienstleistung existiert
        var dienstleistung = await _context.Dienstleistungen.FindAsync(request.DienstleistungID);
        if (dienstleistung == null)
        {
            return BadRequest(new { message = "Ungültige DienstleistungID." });
        }

        // Überprüfen, ob der Kunde bereits existiert
        var existingKunde = await _context.Kunden.FirstOrDefaultAsync(k => k.Email == request.Kunde.Email);

        if (existingKunde != null)
        {
            // Falls Kunde existiert, setze die KundeID anstatt ihn erneut zu erstellen
            request.KundeId = existingKunde.KundeId;
        }
        else
        {
            // Neuer Kunde wird hinzugefügt
            _context.Kunden.Add(request.Kunde);
            await _context.SaveChangesAsync();
            request.KundeId = request.Kunde.KundeId;  // Nach Speicherung, ID setzen
        }

        // 3️⃣ Finalen Auftrag in die Datenbank speichern
        request.Dienstleistung = dienstleistung;  // Verbindung herstellen
        _context.ServiceAuftraege.Add(request);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOffeneAuftraege), new { id = request.AuftragID }, request);
    }


    //  2️⃣ Status eines Auftrags ändern (AUTHENTIFIKATION ERFORDERLICH)
    [Authorize]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateRequest request)
    {
        var auftrag = await _context.ServiceAuftraege.FindAsync(id);

        if (auftrag == null)
        {
            return NotFound(new { message = "Auftrag nicht gefunden." });
        }

        if (request.StatusID < 1 || request.StatusID > 3)
        {
            return BadRequest(new { message = "Ungültiger StatusID. Verwende 1 (Offen), 2 (InArbeit) oder 3 (Abgeschlossen)." });
        }

        auftrag.StatusID = request.StatusID;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Status erfolgreich aktualisiert.", neuerStatus = auftrag.StatusID });
    }


    [HttpDelete("auftrag/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteServiceAuftrag(int id)
    {
        var auftrag = await _context.ServiceAuftraege.FindAsync(id);
        if (auftrag == null)
            return NotFound(new { message = "Serviceauftrag nicht gefunden." });

        _context.ServiceAuftraege.Remove(auftrag);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Serviceauftrag wurde gelöscht." });
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<ServiceAuftrag>>> FilterServiceAuftraege(
    [FromQuery] int? status,
    [FromQuery] int? prioritaet)
    {
        var query = _context.ServiceAuftraege.AsQueryable();

        if (status.HasValue)
            query = query.Where(s => s.StatusID == status.Value);

        if (prioritaet.HasValue)
            query = query.Where(s => s.Priorität == prioritaet.Value);

        var result = await query.ToListAsync();
        return Ok(result);
    }

    [HttpPut("mitarbeiter/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMitarbeiter(int id, [FromBody] MitarbeiterUpdateRequest request)
    {
        var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
        if (mitarbeiter == null)
            return NotFound(new { message = "Mitarbeiter nicht gefunden." });

        mitarbeiter.Name = request.Name;
        mitarbeiter.Email = request.Email;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Mitarbeiter aktualisiert", mitarbeiter });
    }

    [HttpDelete("mitarbeiter/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMitarbeiter(int id)
    {
        var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
        if (mitarbeiter == null)
            return NotFound(new { message = "Mitarbeiter nicht gefunden." });

        _context.Mitarbeiter.Remove(mitarbeiter);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Mitarbeiter gelöscht." });
    }

    public class UpdateStatusRequest
    {
        public int StatusID { get; set; } // 1 = Offen, 2 = InArbeit, 3 = Abgeschlossen
    }

    public class MitarbeiterUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

// DTOs für API-Anfragen
public class StatusUpdateRequest
{
    public int StatusID { get; set; } // 1 = Offen, 2 = InArbeit, 3 = Abgeschlossen
}
