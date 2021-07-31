using Ligak_Optimalis_Kialakitasa.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;

namespace Ligak_Optimalis_Kialakitasa.Models.Validation
{
    public class TeamsValidator
    {
        static public bool Validate(ref IFormCollection ic, ref TournamentRepository tournamentRepository)
        {
            bool l = true;
            string temporary;
            CoordinateHelperLatitude szel;
            CoordinateHelperLongitude hossz;
            string szelkoord;
            string hosszkoord;
            for (int i = 0; i < tournamentRepository.TournamentConstraintsRules.NumberOfTeams; i++)
            {
                if (tournamentRepository.Tournament.Teams[tournamentRepository.TournamentConstraintsRules.NumberOfTeams-1].Name != null)
                {
                    if (tournamentRepository.Tournament.Teams[i].Name == "Hibás név!")
                    {
                        temporary = ic[String.Format("{0}", i + "csapat")];
                        tournamentRepository.Tournament.Teams[i].Name = NameChecker(ref temporary, ref l, tournamentRepository.Tournament.Teams);
                    }
                    else if (tournamentRepository.Tournament.Teams[i].Coordinate.LatitudeValue == -1 || tournamentRepository.Tournament.Teams[i].Coordinate.LongitudeValue == -1)
                    {
                        szelkoord = ic[String.Format("{0}", i + "szelkoordináta")];
                        hosszkoord = ic[String.Format("{0}", i + "hosszkoordináta")];
                        szel = tournamentRepository.Tournament.Teams[i].Coordinate.Latitude;
                        hossz = tournamentRepository.Tournament.Teams[i].Coordinate.Longitude;
                        tournamentRepository.Tournament.Teams[i].Coordinate = CoordinateChecker(ref szelkoord, ref hosszkoord, ref l, ref szel, ref hossz);
                    }
                }
                else
                {
                    temporary = ic[String.Format("{0}", i + "csapat")];
                    szel = ic[String.Format("{0}", i + "Koordinata_szelesseg")] == "Északi" ? CoordinateHelperLatitude.Északi : CoordinateHelperLatitude.Déli;
                    hossz = ic[String.Format("{0}", i + "Koordinata_hosszusag")] == "Keleti" ? CoordinateHelperLongitude.Keleti : CoordinateHelperLongitude.Nyugati;
                    szelkoord = ic[String.Format("{0}", i + "szelkoordináta")];
                    hosszkoord = ic[String.Format("{0}", i + "hosszkoordináta")];
                    tournamentRepository.Tournament.Teams[i].Name = NameChecker(ref temporary, ref l, tournamentRepository.Tournament.Teams);
                    tournamentRepository.Tournament.Teams[i].Coordinate = CoordinateChecker(ref szelkoord, ref hosszkoord, ref l, ref szel, ref hossz);
                }
            }
            return l;
        }

        private static Coordinate CoordinateChecker(ref string t, ref string t2, ref bool l, ref CoordinateHelperLatitude szel, ref CoordinateHelperLongitude hossz)
        {
            if (Regex.IsMatch(t, @"^[1-9][0-9],[0-9][0-9][0-9]"))
            {
                if (Regex.IsMatch(t2, @"^[1-9][0-9],[0-9][0-9][0-9]"))
                {
                    float value = float.Parse(t);
                    if (value >= 0 && value <= 90)
                    {

                        float value2 = float.Parse(t2);
                        if (value >= 0 && value <= 180)
                            return new Coordinate(value, value2) { Latitude = szel, Longitude = hossz };
                    }
                }
            }

            l = false;
            return new Coordinate(-1, -1) { Latitude = szel, Longitude = hossz };
        }

        private static string NameChecker(ref string temporary, ref bool l, Team[] teams)
        {
            if (Regex.IsMatch(temporary, @"^[a-zA-Z0-9áéíóőúűÁÉÍÓŐÚŰ]", RegexOptions.IgnoreCase))
            {
                int i = 0;
                while (i < teams.Length)
                {
                    if (temporary == teams[i].Name)
                    {
                        l = false;
                        return "Hibás név!";
                    }
                    i++;
                }
                return temporary;
            }
            l = false;
            return "Hibás név!";
        }
    }
}
