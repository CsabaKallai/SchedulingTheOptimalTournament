using System.ComponentModel;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Tournament
    {
        public Tournament()
        {

        }
        public Tournament(ref TournamentConstraintsAndRules tcr)
        {
            this.Teams = new Team[tcr.NumberOfTeams];
            for (int i = 0; i < tcr.NumberOfTeams; i++)
            {
                Teams[i] = new Team();
            }
        }

        
        [DisplayName("Bajnokság neve")]
        public string NameOfTournament { get; set; }
        public Team[] Teams { get; set; }
    }
}
