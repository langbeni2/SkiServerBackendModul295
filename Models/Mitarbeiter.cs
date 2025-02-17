namespace SkiServerBackend.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Mitarbeiter
    {
        [Key] // Primärschlüssel für Entity Framework
        public int MitarbeiterID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswortHash { get; set; }  // Gehashtes Passwort
    }


}
