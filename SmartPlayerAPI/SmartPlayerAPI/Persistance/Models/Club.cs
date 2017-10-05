using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateOfCreate { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
