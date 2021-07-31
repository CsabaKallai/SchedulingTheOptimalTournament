using System;
using System.ComponentModel.DataAnnotations;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public enum CoordinateHelperLatitude
    {
        Északi,
        Déli
    }
    public enum CoordinateHelperLongitude
    {
        Keleti,
        Nyugati
    }

    
    public class Coordinate
    {
        private float latitudeValue;
        private float longitudeValue;
        private double latitudeValueInRadian;

        public Coordinate()
        {
        }
        public Coordinate(float latitude, float longitude)
        {
            this.latitudeValue = latitude;
            this.longitudeValue = longitude;
            this.LatitudeValueInRadian = latitudeValue;
        }

        
        [Range(0, 90, ErrorMessage = "Ez a mező 0 - 90 között kell legyen.")]
        [Required(ErrorMessage = "Ezt a mezőt kötelező megadni")]
        public float LatitudeValue
        {
            get
            {
                return latitudeValue;
            }
            set
            {
                latitudeValue = value;
            }
        }

        [Range(0,180,ErrorMessage ="Ez a mező 0 - 180 között kell legyen.")]
        [Required(ErrorMessage = "Ezt a mezőt kötelező megadni")]
        public float LongitudeValue
        {
            get
            {
                return longitudeValue;
            }
            set
            {
                longitudeValue = value;
            }

        }

        public double LatitudeValueInRadian
        {
            get
            {
                return latitudeValueInRadian;
            }
            set
            {
                latitudeValueInRadian =  Math.Cos(latitudeValue * 0.01745329251994329576923690768489);
            }
        }

        public CoordinateHelperLatitude Latitude { get; set; }
        public CoordinateHelperLongitude Longitude { get; set; }
    }
}
