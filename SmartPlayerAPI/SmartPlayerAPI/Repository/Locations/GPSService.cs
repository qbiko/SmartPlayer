using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Locations
{
    public class GPSService
    {
        public double GetDistanceBeetweenPoints(double lat, double lng)
        {
            GeoCoordinate leadCoordinate = new GeoCoordinate(54.377852, 18.607646);
            GeoCoordinate activityCoordinate = new GeoCoordinate(54.377355, 18.609406);
            var distance = leadCoordinate.GetDistanceTo(activityCoordinate);
            return distance;
        }
    }
}
