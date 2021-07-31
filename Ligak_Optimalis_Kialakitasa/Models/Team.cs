using System.ComponentModel.DataAnnotations;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Team
    {
        public Team()
        {
            this.Coordinate = new Coordinate();
        }


        [Required(ErrorMessage ="Ezt a mezőt kötelező megadni")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ezt a mezőt kötelező megadni")]
        public Coordinate Coordinate { get; set; }

        
    }
}
