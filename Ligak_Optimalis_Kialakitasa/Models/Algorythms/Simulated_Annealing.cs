using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligak_Optimalis_Kialakitasa.Models.Algorythms
{
    public class Simulated_Annealing
    {
        static Random RANDOM = new Random();
        private Result result;
        private string[] array;
        private Tournament tournament;
        private TournamentConstraintsAndRules tournamentConstraintsAndRules;

        public Simulated_Annealing(ref Result bts, ref Tournament tournament, ref TournamentConstraintsAndRules tournamentConstraintsAndRules)
        {
            this.result = bts;
            this.tournament = tournament;
            this.tournamentConstraintsAndRules = tournamentConstraintsAndRules;
        }


        public static Result Result { get; private set; }

        public void GenerateTournament()
        {
            this.SimulatedAnnealing();
        }

        private void SimulatedAnnealing()
        {
            List<Round> Scheduling = result.TournamentSchedule;

            List<Round> Scheduling_ = new List<Round>();

            double bestSoFar = result.GoodnessValue;
            double T;
            int counter = 0, phase = 0, maxC, maxP;
            bool accept = false;
            double e = 0.0, B = 0.9999, f_0 = 0.0, f_1 = 0.0;

            if (tournamentConstraintsAndRules.NumberOfTeams < 9)
            {
                T = 400;
                maxC = 500;
                maxP = 100;
            }
            else if (tournamentConstraintsAndRules.NumberOfTeams < 13 && tournamentConstraintsAndRules.NumberOfTeams > 8)
            {
                T = 600;
                maxC = 400;
                maxP = 125;
            }
            else
            {
                T = 700;
                maxC = 100;
                maxP = 150;
            }


            while (phase <= maxP)
            {
                counter = 0;
                while (counter <= maxC)
                {
                    Scheduling_ = GenerateModifiedSchedule(Scheduling);
                    f_0 = f(Scheduling_);
                    f_1 = f(Scheduling);
                    if (f_0 < f_1)
                    {
                        accept = true;
                    }
                    else
                    {
                        e = f_1 - f_0;
                        if (Simulated_Annealing.RANDOM.NextDouble() < Math.Exp(-e / T))
                        {
                            accept = true;
                        }
                        else
                        {
                            accept = false;
                        }

                    }
                    if (accept)
                    {
                        Scheduling = Scheduling_;
                        if (f_0 < bestSoFar)
                        {
                            counter = 0;
                            //phase = 0;
                            bestSoFar = f_0;
                        }
                        else
                        {
                            counter++;
                        }
                    }
                }
                phase++;
                T *= B;
            }
            result.GoodnessValue = double.Parse(String.Format("{0:0.0000}", bestSoFar));
            result.TournamentSchedule.Sort(delegate (Round x, Round y)
            {
                return x.NumberOfRound.CompareTo(y.NumberOfRound);
            });
            Result = result;
        }

        

        private List<Round> GenerateModifiedSchedule(List<Round> scheduling)
        {
            int half = tournament.Teams.Length / 2;
            int first = Simulated_Annealing.RANDOM.Next(0, half);
            int second = Simulated_Annealing.RANDOM.Next(half, tournament.Teams.Length);

            
            List<Round> r = scheduling;
            switch (Simulated_Annealing.RANDOM.Next(0, 2))
            {
                case 0:
                    SwapHomes(ref r, first, second);
                    break;

                case 1:
                    SwapRounds(ref r);
                    break;
            }

            return r;
        }
        private void SwapRounds(ref List<Round> scheduling)
        {
            if (tournamentConstraintsAndRules.Robins == Robins.Double_Round_Robin)
            {
                int first = RANDOM.Next(0, scheduling.Count / 4);
                int second = RANDOM.Next(scheduling.Count / 4, scheduling.Count / 2);

                Round temporary = scheduling[second];
                scheduling[second] = scheduling[first];
                scheduling[first] = temporary;

                first = 0;
                for (int i = scheduling.Count / 2; i < scheduling.Count; i++)
                {
                    for (int j = 0; j < scheduling[first].MatchesOfRound.Length; j++)
                    {
                        scheduling[i].MatchesOfRound[j] = SwapTeams(ref scheduling[first].MatchesOfRound[j]);
                    }
                    first++;
                }
            }
            else
            {
                int first = RANDOM.Next(0, scheduling.Count / 2);
                int second = RANDOM.Next(scheduling.Count / 2, scheduling.Count);
                Round temporary = scheduling[second];
                scheduling[second] = scheduling[first];
                scheduling[first] = temporary;
            }
        }

        private double f(List<Round> scheduling_)
        {
            int k = -1;
            int index = 0;
            List<List<int>> opponents = new List<List<int>>();


            foreach (var teamx in tournament.Teams)
            {
                opponents.Add(new List<int>());
                foreach (var match in this.result.TournamentSchedule.Select(x => x.MatchesOfRound))
                {
                    foreach (var item in match)
                    {
                        if (item.Contains(teamx.Name))
                        {
                            array = item.Split(':');
                            for (int i = 0; i < array.Length; i++)
                            {
                                array[i] = array[i].TrimEnd();
                                array[i] = array[i].TrimStart();
                            }

                            if (k == -1)
                            {
                                if (array[0] == teamx.Name)
                                {
                                    k = 0;
                                    opponents[index].Add(FindIndexByTeamName(array[0]));
                                    //opponents[index].Add(teamx);
                                }
                                else
                                {
                                    k = 1;
                                    //opponents[index].Add(teamx);
                                    opponents[index].Add(FindIndexByTeamName(array[1]));
                                    opponents[index].Add(FindIndexByTeamName(array[0]));
                                    //opponents[index].Add(tournament.Teams.First(x => x.Name == array[0]));
                                }
                                continue;
                            }


                            if (k == 1 && array[0] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(array[0]));
                                //opponents[index].Add(teamx);
                                k = 0;
                            }
                            else if (k == 1 && array[1] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(array[0]));
                                //opponents[index].Add(tournament.Teams.First(x => x.Name == array[0]));
                                k = 1;
                            }
                            else if (k == 0 && array[1] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(array[0]));
                                //opponents[index].Add(tournament.Teams.First(x => x.Name == array[0]));
                                k = 1;
                            }

                        }
                    }
                }
                if (k == 1)
                {
                    opponents[index].Add(FindIndexByTeamName(teamx.Name));
                    //opponents[index].Add(teamx);
                }
                index++;
                k = -1;
            }

            double distance = 0.0;
            foreach (var item in opponents)
            {
                for (int i = 0; i < item.Count - 1; i++)
                {
                    distance += Models.Haversine.CalculateDistanceBetweenTwoCoordinates(item[i], item[i + 1]);
                }
            }
            return distance;
        }
        private int FindIndexByTeamName(string v)
        {
            for (int i = 0; i < tournament.Teams.Length; i++)
            {
                if (tournament.Teams[i].Name == v)
                {
                    return i;
                }
            }
            return 0;
        }
        private string SwapTeams(ref string v)
        {
            array = v.Split(':');
            return String.Format("  {0}  :  {1}  ", array[1], array[0]);
        }

        private void SwapHomes(ref List<Round> scheduling, int first, int second)
        {
            Team first_team = tournament.Teams[first];
            Team second_team = tournament.Teams[second];



            if (tournamentConstraintsAndRules.Robins == Robins.Double_Round_Robin)
            {
                int x = 0;
                int y = 0;


                foreach (var item in scheduling)
                {
                    foreach (var match in item.MatchesOfRound)
                    {
                        if (match.Contains(first_team.Name) && match.Contains(second_team.Name))
                        {
                            string[] h = scheduling[x].MatchesOfRound[y].Split(':');
                            scheduling[x].MatchesOfRound[y] = string.Format("{0}:{1}", h[1], h[0]);
                            h = scheduling[x + scheduling.Count / 2].MatchesOfRound[y].Split(':');
                            scheduling[x + scheduling.Count / 2].MatchesOfRound[y] = string.Format("{0}:{1}", h[1], h[0]);
                            return;
                        }
                        y++;
                    }
                    y = 0;
                    x++;
                }
            }
            else
            {
                int x = 0;
                int y = 0;

                foreach (var item in scheduling)
                {
                    foreach (var match in item.MatchesOfRound)
                    {
                        if (match.Contains(first_team.Name) && match.Contains(second_team.Name))
                        {
                            string[] h = scheduling[x].MatchesOfRound[y].Split(':');
                            scheduling[x].MatchesOfRound[y] = string.Format("{0}:{1}", h[1], h[0]);
                            return;
                        }
                        y++;
                    }
                    y = 0;
                    x++;
                }
            }
        }
    }
}
