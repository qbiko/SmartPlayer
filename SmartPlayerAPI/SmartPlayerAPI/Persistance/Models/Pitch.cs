using SmartPlayerAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Pitch : IAggregate
    {
        public int Id { get; set; }
        public string NameOfPitch { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string LeftUpPoint { get; set; }
        public string LeftDownPoint { get; set; }
        public string RightUpPoint { get; set; }
        public string RightDownPoint { get; set; }
    }
}
