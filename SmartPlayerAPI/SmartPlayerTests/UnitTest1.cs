using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartPlayerAPI.Repository.Locations;

namespace SmartPlayerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGeoCoordinates()
        {
            var service = new GPSService();
            var result = service.GetCartesianPoint(new SmartPlayerAPI.ViewModels.Pitch.PitchCornersPoints()
            {
                LeftUpPoint = new GPSPoint(54.370416, 18.630052),
                LeftDownPoint = new GPSPoint(54.370014, 18.629365),
                RightUpPoint = new GPSPoint(54.369786, 18.631127),
                RightDownPoint = new GPSPoint(54.369383, 18.630432)
            }, new GPSPoint(54.369568, 18.630247));
        }
    }
}
