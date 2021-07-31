using System.Collections.Generic;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Result
    {
        public Result()
        {
            this.TournamentSchedule = new List<Round>();
        }
        public List<Round> TournamentSchedule  { get; set; }
        public double GoodnessValue { get; set; }
        public string NameOfTournament { get; set; }

        
    }
}
