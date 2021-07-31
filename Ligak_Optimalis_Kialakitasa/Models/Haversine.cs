using System;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class Haversine
    {
        private static float[,] DISTANCESBETWEENPLACES;

        public Haversine(Tournament tournament)
        {
            DISTANCESBETWEENPLACES = new float[tournament.Teams.Length, tournament.Teams.Length];
            int index = 0;
            int k = 0;
            int h = 0;
            for (int i = 0; i < tournament.Teams.Length-1; i++)
            {
                k = i + 1;
                for (int j = i ; j < tournament.Teams.Length; j++)
                {
                    if (i != index)
                    {
                        DISTANCESBETWEENPLACES[i, index] = CalculateDistanceBetweenTwoCoordinates(tournament.Teams[i].Coordinate, tournament.Teams[j].Coordinate);
                        DISTANCESBETWEENPLACES[k++, h] = DISTANCESBETWEENPLACES[i, index];
                    }
                    index++;
                }
                h++;
                index = h;
            }
        }

        private static float CalculateDistanceBetweenTwoCoordinates(Coordinate origin, Coordinate destination)
        {
            double dLat = 0.00872664625997164788461845384245 * (destination.LatitudeValue - origin.LatitudeValue);
            double dLon = 0.00872664625997164788461845384245 * (destination.LongitudeValue - origin.LongitudeValue);

            double a = Math.Sin(dLat) * Math.Sin(dLat) + Math.Sin(dLon) * Math.Sin(dLon) * origin.LatitudeValueInRadian * destination.LatitudeValueInRadian;
            float c = (float)(12745.6 * Math.Asin(Math.Sqrt(a)));
            return c;
        }

        public static float CalculateDistanceBetweenTwoCoordinates(int originindex, int destinationindex)
        {
            return DISTANCESBETWEENPLACES[originindex,destinationindex];
        }
    }
}
