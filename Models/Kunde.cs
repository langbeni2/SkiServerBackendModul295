namespace SkiServerBackend.Models
{
    
        public class Kunde
        {
            public int KundeId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Telefon { get; set; }

            public ICollection<ServiceAuftrag>? ServiceAuftraege { get; set; } // ← Optional durch `?`
        }

    

}
