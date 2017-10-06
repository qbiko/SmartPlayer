using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class AccelerometerAndGyrocopeModel
    {
        public List<int> Integers { get; set; }
    }
    public class SensorsDataViewModel
    {
        public int PulseSensorValue { get; set; }
        public AccelerometerAndGyrocopeModel AccelerometerAndGyrocopeModel { get; set; }

    }
}
