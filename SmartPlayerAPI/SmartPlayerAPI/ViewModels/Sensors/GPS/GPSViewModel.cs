using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels.Sensors.GPS
{
    public class Point
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    public class GPSViewModel : Point
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
    }

    public class GPSPlayerInGame
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
    }

    public class PointInTime : Point
    {
        public DateTimeOffset TimeOfOccur { get; set; }
    }
}
