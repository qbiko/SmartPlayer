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
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
    public class PointWithMac: GPSViewModel
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
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
}
