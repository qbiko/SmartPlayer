using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int HeighOfUser { get; set; }
        public int WeightOfUser { get; set; }
        public Club Club { get; set; }
        public int ClubId { get; set; }
        public ICollection<PlayerInGame> PlayerInGame { get; set; }
    }
}
