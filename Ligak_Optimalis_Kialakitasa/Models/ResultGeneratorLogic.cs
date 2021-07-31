using Microsoft.Extensions.Primitives;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public static class ResultGeneratorLogic
    {
        static private Result RESULT = new Result();
        static private Tournament TOURNAMENT;
        static private TournamentConstraintsAndRules TOURNAMENTCONSTRAINTANDRULES;
        static private Haversine HAVERSINE;

         public static Result Solve(StringValues optimize_mode)
        {
            if (optimize_mode == "Simulated Annealing")
            {
                Algorythms.Simulated_Annealing SA = new Algorythms.Simulated_Annealing(ref RESULT, ref TOURNAMENT, ref TOURNAMENTCONSTRAINTANDRULES);
                SA.GenerateTournament();
                return Algorythms.Simulated_Annealing.Result;
            }
            else
            {
                Algorythms.GeneticAlgorythm GA = new Algorythms.GeneticAlgorythm(ref TOURNAMENT, ref TOURNAMENTCONSTRAINTANDRULES);
                GA.GenerateTournament();
                return Algorythms.GeneticAlgorythm.Result;
            }
        }

        public static Result Solve(Tournament tournament, TournamentConstraintsAndRules tournamentConstraintsRules)
        {
            if (HAVERSINE == null)
            {
                HAVERSINE = new Haversine(tournament);
                TOURNAMENT = tournament;
                TOURNAMENTCONSTRAINTANDRULES = tournamentConstraintsRules;
            }
            
            Algorythms.Back_Track_Search BTS = new Algorythms.Back_Track_Search(ref tournament, ref tournamentConstraintsRules);
            BTS.GenerateTournament();

            RESULT = BTS.Result;

            return RESULT;
        }
    }
}
