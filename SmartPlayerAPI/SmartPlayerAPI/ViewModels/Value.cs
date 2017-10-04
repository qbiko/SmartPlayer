using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Models
{
    public class Value
    {
        public int Lat { get; set; }
        public int Lng { get; set; }
    }

    public class BatchValues
    {
        public List<Value> Coordinates { get; set; }
        public string UserId { get; set; }
    }
}
