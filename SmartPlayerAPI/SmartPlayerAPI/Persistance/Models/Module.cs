using SmartPlayerAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Module : IAggregate
    {
        public int Id { get; set; }
        public string MACAddress { get; set; }
        public virtual ICollection<PlayerInGame> PlayerInGames { get; set; }
    }
}
