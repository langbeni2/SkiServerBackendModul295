using Microsoft.EntityFrameworkCore;
using SkiServerBackend.Models; // Stelle sicher, dass das richtige Namespace verwendet wird

namespace SkiServerBackend.Data // Falls dein Projekt einen anderen Namespace nutzt, anpassen
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ServiceAuftrag> ServiceAuftraege { get; set; }
        public DbSet<Mitarbeiter> Mitarbeiter { get; set; }

        public DbSet<Dienstleistung> Dienstleistungen { get; set; }

        public DbSet<Kunde> Kunden { get; set; }  // <-- Korrigiert
        

    }
}
