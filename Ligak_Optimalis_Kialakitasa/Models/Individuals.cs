namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Individuals
    {
        private int[,] utazasi_tabla;
        private int[,] ellenfelek;

        public Individuals()
        {
        }

        public Individuals(ref TournamentConstraintsAndRules tournamentConstraintsAndRules)
        {
            int numberOfTeams = tournamentConstraintsAndRules.NumberOfTeams;
            if (tournamentConstraintsAndRules.Robins == Robins.Double_Round_Robin)
            {
                utazasi_tabla = new int[numberOfTeams, (2 * (numberOfTeams - 1))];
                ellenfelek = new int[numberOfTeams,  (2 * (numberOfTeams - 1))];
            }
            else
            {
                utazasi_tabla = new int[numberOfTeams, numberOfTeams - 1];
                ellenfelek = new int[numberOfTeams, numberOfTeams - 1];
            }
        }

        public double GoodnessValue { get; set; }
        public int[,] Utazasi_tabla { get => utazasi_tabla; set => utazasi_tabla = value; }
        public int[,] Ellenfelek { get => ellenfelek; set => ellenfelek = value; }

        
    }
}
