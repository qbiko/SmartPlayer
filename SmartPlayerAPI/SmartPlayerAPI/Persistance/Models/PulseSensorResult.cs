using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class PulseSensorResult
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset TimeOfOccur { get; set; }
        public int PlayerInGameId { get; set; }
        public virtual PlayerInGame PlayerInGame { get; set; }
    }
}
