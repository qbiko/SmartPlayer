using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Locations
{
    public class GPSService
    {
        public double GetDistanceBeetweenPoints(GPSPoint point1, GPSPoint point2)
        {
            GeoCoordinate leadCoordinate = new GeoCoordinate(point1.Lat, point1.Lng);
            GeoCoordinate activityCoordinate = new GeoCoordinate(point2.Lat, point2.Lng);
            var distance = leadCoordinate.GetDistanceTo(activityCoordinate);
            return distance;
        }
    }
}
