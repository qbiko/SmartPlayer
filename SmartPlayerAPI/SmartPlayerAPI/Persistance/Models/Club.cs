using Newtonsoft.Json;
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
        public string PasswordHash { get; set; }
        public DateTimeOffset DateOfCreate { get; set; }
        [JsonIgnore]
        public virtual ICollection<Player> Players { get; set; }
        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
        [JsonIgnore]
        public virtual ICollection<Module> Modules { get; set; }
    }
}
