using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Locations
{
    public class GPSPoint
    {
        public GPSPoint(double Lat, double Lng)
        {
            this.Lat = Lat;
            this.Lng = Lng;
        }
        public double Lat { get; set; }
        public double Lng { get;set; }
    }
}
