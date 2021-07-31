using System;
using System.Collections.Generic;
using System.Linq;

namespace Ligak_Optimalis_Kialakitasa.Models.Algorythms
{
    public class Back_Track_Search
    {
        static Random RANDOM = new Random();
        static private string[] ARRAY;
        private Tournament tournament;
        private TournamentConstraintsAndRules tournamentConstraintsRules;

        public Back_Track_Search(ref Tournament tournament, ref TournamentConstraintsAndRules tournamentConstraintsRules)
        {
            this.tournament = tournament;
            this.tournamentConstraintsRules = tournamentConstraintsRules;
        }


        public Result Result { get; private  set; }

        public void GenerateTournament()
        {
            BackTrackSearch();
        }

        private void  BackTrackSearch()
        {
            bool van = false;
            Result result = null;

            Round round;
            int meccs = 0;
            TeamServicer teamServicer = new TeamServicer();


            int _round = 0;
            int N = (int)(tournamentConstraintsRules.NumberOfTeams / 2) * (tournamentConstraintsRules.NumberOfTeams - 1);
            int limit = 30000;
            const int base_number = 30000;
            int multiplier = 1;

            while (!van)
            {
                result = new Result();
                round = new Round(tournamentConstraintsRules);
                teamServicer.Teams = GenerateMatch(tournament.Teams, ref tournamentConstraintsRules);

                BTS_single(ref meccs, ref _round, ref teamServicer, ref van, ref result, ref N, ref tournamentConstraintsRules, ref round, ref limit);
                teamServicer.RemovedTeams.Clear();
                limit += (base_number * multiplier);
                multiplier++;
                _round = 0;
                meccs = 0;
            }

            if (tournamentConstraintsRules.Robins == Robins.Double_Round_Robin)
            {
                ConvertToDRR(ref result);
            }
            this.Result = result;
            this.Result.GoodnessValue = Cost();
            this.Result.NameOfTournament = tournament.NameOfTournament;
        }

        private static void ConvertToDRR(ref Result result)
        {
            Round r;
            int index = 0;
            int k = result.TournamentSchedule.Count;
            int round = result.TournamentSchedule[result.TournamentSchedule.Count - 1].NumberOfRound + 1;
            while (index < k)
            {
                r = new Round(result.TournamentSchedule[0].MatchesOfRound.Length);
                r.NumberOfRound = round++;
                for (int i = 0; i < r.MatchesOfRound.Length; i++)
                {
                    r.MatchesOfRound[i] = SwapTeams(ref result.TournamentSchedule[index].MatchesOfRound[i]);
                }
                result.TournamentSchedule.Add(r);
                index++;
            }
        }

        private static List<string> GenerateMatch(Team[] teams, ref TournamentConstraintsAndRules tcr)
        {
            GenerateAllMatchByTeamsName(in teams, in tcr);

            List<string> mixedMatches = new List<string>();
            MixingMatches(tcr, ref mixedMatches);


            List<int> szamok = new List<int>();
            ShuffleNumbersForRandomOrder(mixedMatches, szamok);

            List<string> matchesBasedOnrandomOrder = new List<string>();
            for (int i = 0; i < szamok.Count; i++)
            {
                matchesBasedOnrandomOrder.Add(mixedMatches[szamok[i]]);
            }
            return matchesBasedOnrandomOrder;
        }

        private static void ShuffleNumbersForRandomOrder(List<string> mixedMatches, List<int> szamok)
        {
            int k = 0;
            int count = mixedMatches.Count;
            int min = 0;
            int max = count;
            while (count != 0)
            {
                k = Back_Track_Search.RANDOM.Next(min, max);
                if (!szamok.Contains(k))
                {
                    int s = 0;
                    szamok.Add(k);
                    if (k == min)
                    {
                        do
                        {
                            s++;
                        } while (szamok.Contains(++k));
                        if (s == 1)
                        {
                            min = k;
                        }
                        else
                        {
                            min = k;
                        }

                    }
                    s = 0;
                    if (max - 1 == k)
                    {
                        do
                        {
                            s++;
                        } while (szamok.Contains(--k));
                        if (k > min)
                        {
                            if (s == 1)
                            {
                                max = k + 1;
                            }
                            else
                            {
                                max = k + 1;
                            }
                        }
                    }
                    count--;
                }
            }
        }

        private static void MixingMatches(TournamentConstraintsAndRules tcr, ref List<string> m)
        {
            int db = 0;
            for (int i = 0; i < ARRAY.Length; i++)
            {
                if (ARRAY[i] != null)
                {
                    string[] j = ARRAY[i].Split(':');
                    j[0] = j[0].Trim();
                    j[1] = j[1].Trim();
                    for (int s = i + (tcr.NumberOfTeams - 1); s < ARRAY.Length; s++)
                    {
                        if (ARRAY[s] != null)
                        {
                            string[] l = ARRAY[s].Split(':');
                            l[0] = l[0].Trim();
                            l[1] = l[1].Trim();
                            if (String.Format("{0}{1}", j[0], j[1]) == String.Format("{0}{1}", l[1], l[0]))
                            {
                                if (db == 0)
                                {
                                    m.Add(ARRAY[i]);
                                    db = 1;
                                }
                                else
                                {
                                    m.Add(ARRAY[s]);
                                    db = 0;
                                }
                                ARRAY[s] = null;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateAllMatchByTeamsName(in Team[] teams, in TournamentConstraintsAndRules tcr)
        {
            ARRAY = new string[tcr.NumberOfTeams * (tcr.NumberOfTeams - 1)];
            int index = 0;
            for (int i = 0; i < teams.Length; i++)
            {
                for (int s = 0; s < teams.Length; s++)
                {
                    if (s != i)
                    {
                        ARRAY[index++] = String.Format("  {0}  :  {1}  ", teams[i].Name, teams[s].Name);
                    }

                }
            }
        }

        private double Cost()
        {
            int k = -1;
            int index = 0;
            List<List<int>> opponents = new List<List<int>>();

            
            foreach (var teamx in tournament.Teams)
            {
                opponents.Add(new List<int>());
                foreach (var match in this.Result.TournamentSchedule.Select(x => x.MatchesOfRound))
                {
                    foreach (var item in match)
                    {
                        if (item.Contains(teamx.Name))
                        {
                            ARRAY = item.Split(':');
                            for (int i = 0; i < ARRAY.Length; i++)
                            {
                                ARRAY[i] = ARRAY[i].TrimEnd();
                                ARRAY[i] = ARRAY[i].TrimStart();
                            }

                            if (k == -1)
                            {
                                if (ARRAY[0] == teamx.Name)
                                {
                                    k = 0;
                                    opponents[index].Add(FindIndexByTeamName(ARRAY[0]));
                                    //opponents[index].Add(teamx);
                                }
                                else
                                {
                                    k = 1;
                                    //opponents[index].Add(teamx);
                                    opponents[index].Add(FindIndexByTeamName(ARRAY[1]));
                                    opponents[index].Add(FindIndexByTeamName(ARRAY[0]));
                                    //opponents[index].Add(tournament.Teams.First(x => x.Name == array[0]));
                                }
                                continue;
                            }


                            if (k == 1 && ARRAY[0] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(ARRAY[0]));
                                //opponents[index].Add(teamx);
                                k = 0;
                            }
                            else if (k == 1 && ARRAY[1] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(ARRAY[0]));
                                //opponents[index].Add(tournament.Teams.First(x => x.Name == array[0]));
                                k = 1;
                            }
                            else if (k == 0 && ARRAY[1] == teamx.Name)
                            {
                                opponents[index].Add(FindIndexByTeamName(ARRAY[0]));
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
            return double.Parse(String.Format("{0:0.0000}", distance));
        }

        private int FindIndexByTeamName(string team)
        {
            for (int i = 0; i < tournament.Teams.Length; i++)
            {
                if (tournament.Teams[i].Name == team)
                {
                    return i;
                }
            }
            return 0;
        }

        private static double Haversine(Coordinate origin, Coordinate destination)
        {
            //double distance_latitude;
            //double distance_longitude;

            //if (origin.Latitude != destination.Latitude)
            //{
            //    distance_latitude = destination.LatitudeValue + origin.LatitudeValue;
            //    if (origin.Longitude != destination.Longitude)
            //    {
            //        distance_longitude = destination.LongitudeValue + origin.LongitudeValue;
            //    }
            //    else
            //    {
            //        distance_longitude = destination.LongitudeValue - origin.LongitudeValue;
            //    }
            //}
            //else
            //{
            //    distance_latitude = destination.LatitudeValue - origin.LatitudeValue;
            //    if (origin.Longitude != destination.Longitude)
            //    {
            //        distance_longitude = destination.LongitudeValue + origin.LongitudeValue;
            //    }
            //    else
            //    {
            //        distance_longitude = destination.LongitudeValue - origin.LongitudeValue;
            //    }
            //}
            return 0.0;
        }

        private static string SwapTeams(ref string match)
        {
            Back_Track_Search.ARRAY = match.Split(':');
            return String.Format("  {0}  :  {1}  ", ARRAY[1], ARRAY[0]);
        }


        private static void BTS_single(ref int numberOfAllocatedMatches, ref int serialNumberOfTheRound, ref TeamServicer allocableMatches, ref bool IsFinished, ref Result result, ref int numberOfAllocatableMatches, ref TournamentConstraintsAndRules tcr, ref Round _round, ref int limit)
        {
            int index = 0;
            while (!IsFinished && index < allocableMatches.Teams.Count && limit != 0)
            {
                if (IsTheMatchAcceptable(allocableMatches.Teams[index], ref _round))
                {
                    _round.AddMatch(allocableMatches.Teams[index]);
                    allocableMatches.RemovedTeams.Push(allocableMatches.Teams[index]);
                    allocableMatches.Teams.Remove(allocableMatches.Teams[index]);
                    numberOfAllocatedMatches++;

                    if (_round.IsFull())
                    {
                        result.TournamentSchedule.Add(_round);
                        _round = new Round(tcr);
                        serialNumberOfTheRound++;
                        _round.NumberOfRound = serialNumberOfTheRound;
                    }
                    BTS_single(ref numberOfAllocatedMatches, ref serialNumberOfTheRound, ref allocableMatches, ref IsFinished, ref result, ref numberOfAllocatableMatches, ref tcr, ref _round, ref limit);
                    if (numberOfAllocatedMatches == numberOfAllocatableMatches)
                    {
                        IsFinished = true;
                    }
                    else
                    {
                        if (_round.RemoveMatch())
                        {
                            allocableMatches.Teams.Insert(index, allocableMatches.RemovedTeams.Pop());
                            numberOfAllocatedMatches--;
                        }
                        else
                        {
                            _round = result.TournamentSchedule.Last();
                            result.TournamentSchedule.Remove(_round);
                            _round.RemoveMatch();
                            serialNumberOfTheRound--;
                            numberOfAllocatedMatches--;
                            allocableMatches.Teams.Insert(index, allocableMatches.RemovedTeams.Pop());
                        }
                    }
                    limit--;
                }
                index++;
            }
        }


        private static bool IsTheMatchAcceptable(string match, ref Round round)
        {
            Back_Track_Search.ARRAY = match.Split(':');
            for (int i = 0; i < round.Index; i++)
            {
                if (round.MatchesOfRound[i].Contains(ARRAY[0]) || round.MatchesOfRound[i].Contains(ARRAY[1]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
