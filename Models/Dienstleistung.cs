using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkiServerBackend.Models
{
   
    
        public class Dienstleistung
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DienstleistungID { get; set; }

            
            public string Name { get; set; }
        }


    

}
