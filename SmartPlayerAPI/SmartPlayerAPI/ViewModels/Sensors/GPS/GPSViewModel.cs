using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels.Sensors.GPS
{
    public class PointInTime : Point
    {
        public DateTimeOffset TimeOfOccur { get; set; }
    }
    public class Point
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    public class PointWithCredentials : GPSPlayerInGame
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    public class GPSViewModel
    {
        public string ModuleMac { get; set; }
    }

    public class GPSPlayerInGame
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
    }

    public class GPSPlayerInGameInTime : GPSPlayerInGame
    {
        public string StartDateString { get; set; }
    }
    public class CartesianPointsInTime
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTimeOffset TimeOfOccur { get; set; }
    }

    public class GeoPointsInTime
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double TimeOfOccurLong { get; set; }
    }

    public class GPSSensorInBatch
    {
        public int Value { get; set; }
        public double TimeOfOccurLong { get; set; }
    }

    public class GPSBatch<T>
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public List<T> ListOfPositions { get; set; } = new List<T>();

    }



}