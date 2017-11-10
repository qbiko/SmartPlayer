using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels.Modules
{
    public class ModuleIn
    {
        public string MACAddress { get; set; }
    }
    public class ModuleOut : ModuleIn
    {
        public int Id { get; set; }
    }
}
