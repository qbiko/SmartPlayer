using GeoCoordinatePortable;
using SmartPlayerAPI.ViewModels.Pitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Locations
{
    public class CartesianPoint
    {
        public CartesianPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class GPSService
    {
        private readonly int MaxX = 601;
        private readonly int MaxY = 389;
        public double GetDistanceBeetweenPoints(GPSPoint point1, GPSPoint point2)
        {
            GeoCoordinate leadCoordinate = new GeoCoordinate(point1.Lat, point1.Lng);
            GeoCoordinate activityCoordinate = new GeoCoordinate(point2.Lat, point2.Lng);
            var distance = leadCoordinate.GetDistanceTo(activityCoordinate);
            return distance;
        }

        public CartesianPoint GetCartesianPoint(PitchCornersPoints pitchCornersPoints, GPSPoint targetPoint)
        {
            var a1 = GetDistanceBeetweenPoints(pitchCornersPoints.LeftUpPoint, pitchCornersPoints.LeftDownPoint); //max y
            var b1 = GetDistanceBeetweenPoints(pitchCornersPoints.LeftUpPoint, targetPoint);
            var c1 = GetDistanceBeetweenPoints(pitchCornersPoints.LeftDownPoint, targetPoint);
            var c2 = GetDistanceBeetweenPoints(pitchCornersPoints.RightUpPoint, targetPoint);
            var a2 = GetDistanceBeetweenPoints(pitchCornersPoints.LeftUpPoint, pitchCornersPoints.RightUpPoint); //max x

            double p1 = (a1 + b1 + c1) / 2;
            var w1 = p1 * (p1 - a1) * (p1 - b1) * (p1 - c1);
            var h1 = (2 * Math.Sqrt(w1)) / a1; //x distance on map

            double p2 = (a2 + b1 + c2) / 2;
            var w2 = p2 * (p2 - a2) * (p2 - b1) * (p2 - c2);
            var h2 = (2 * Math.Sqrt(w2)) / a2; //y distance on map

            double scaleOnYAxis = h2 / a1;
            double scaleOnXAxis = h1 / a2;

            double x = scaleOnXAxis * MaxX;
            double y = scaleOnYAxis * MaxY;

            return new CartesianPoint(x, y);
        }
    }
}
