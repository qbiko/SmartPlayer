using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class PlayerInGame
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public bool Active { get; set; }
        public int? GameId { get; set; }
        public int? PlayerId { get; set; }
        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<PulseSensorResult> PulseSensorResults { get; set; }
        public virtual ICollection<AccelerometerAndGyroscopeResult> AccelerometerAndGyroscopeResults { get; set; }
    }
}
