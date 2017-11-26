using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class GameViewModelIn
    {
        public string NameOfGame { get; set; }
        public DateTimeOffset TimeOfStart { get; set; }
        public int ClubId { get; set; }
        public int PitchId { get; set; }
    }
    public class GameViewModelOut : GameViewModelIn
    {
        public int Id { get; set; }
    }
}
