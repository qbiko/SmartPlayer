using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string NameOfGame { get; set; }
        public DateTimeOffset TimeOfStart { get; set; }
        public Club Club { get; set; }
        public int ClubId { get; set; }
        public ICollection<PlayerInGame> PlayerInGame { get; set; }
    }
}
