namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Round
    {
        
        public Round(TournamentConstraintsAndRules tcr)
        {
            int round = tcr.NumberOfTeams / 2;
            this.MatchesOfRound = new string[round];
        }
        public Round(int round)
        {
            this.MatchesOfRound = new string[round];
        }

        
        public string[] MatchesOfRound { get; set; }
        public int NumberOfRound { get; set; }
        public int Index { get; private set; }

        public void AddMatch(string team1)
        {
            MatchesOfRound[Index++] = team1;
        }
        public bool RemoveMatch()
        {
            Index--;
            if (Index > -1)
            {
                MatchesOfRound[Index] = null;
            }
            return Index != -1;
        }
        public bool IsFull()
        {
            return Index == MatchesOfRound.Length;
        }
    }
}
