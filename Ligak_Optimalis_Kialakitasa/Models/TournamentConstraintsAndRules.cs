using System.ComponentModel;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    
    public enum Robins
    {
        Single_Round_Robin,
        Double_Round_Robin
    }

    public class TournamentConstraintsAndRules
    {
        [DisplayName("Ütemezési technika")]
        public  Robins Robins { get; set; }

        [DisplayName("Csapatok darabszáma")]
        public  int NumberOfTeams { get; set; }

        [DisplayName("Idegenben játszható meccsek száma egyhuzamban")]
        public  int NumberOfMatchesPlayedInAAwayGame { get; set; }

    }
}
