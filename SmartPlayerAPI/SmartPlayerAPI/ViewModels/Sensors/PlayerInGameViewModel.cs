using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels.Sensors
{ 
    public class PlayerInGameViewModel
    {
        public int? PlayerId { get; set; }
        public int? GameId { get; set; }
        public long ServerTime { get; set; }
        public DateTimeOffset Now { get; set; }
        public DateTimeOffset StartGame { get; set; }
    }
}
