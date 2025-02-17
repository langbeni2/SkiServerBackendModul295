using SkiServerBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ServiceAuftrag
{
    [Key]
    [Column("AuftragID")]
    public int AuftragID { get; set; }
    public int KundeId { get; set; } // Fremdschlüssel

    public int MitarbeiterID { get; set; }
    public int DienstleistungID { get; set; }
    public int StatusID { get; set; }       
    public int? Priorität { get; set; }
    public DateTime? Erstellungsdatum { get; set; }

    // Navigation Property
    public Kunde Kunde { get; set; } // Navigation zur Kunden-Tabelle

    // Konstruktor
    public ServiceAuftrag()
    {
        Kunde = new Kunde();
    }

    // Alias für AuftragID
    public int Id => AuftragID;
    public Dienstleistung? Dienstleistung { get; set; }  
}
