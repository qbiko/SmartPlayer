using SmartPlayerAPI.Repository.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels.Pitch
{
    public class PitchIn
    {
        public string NameOfPitch { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public GPSPoint LeftUpPoint { get; set; }
        public GPSPoint LeftDownPoint { get; set; }
        public GPSPoint RightUpPoint { get; set; }
        public GPSPoint RightDownPoint { get; set; }
    }
    public class PitchOut : PitchIn
    {
        public int Id { get; set; }
    }
}
