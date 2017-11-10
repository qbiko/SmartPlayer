using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class PlayerInGameViewModelOutExtend : PlayerInGameViewModelOut
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
    public class PlayerInGameViewModelOut : PlayerInGameViewModelIn
    {
        public int Id { get; set; }
    }
    public class PlayerInGameViewModelIn
    {
        public string Position { get; set; }
        public int Number { get; set; }
        public bool Active { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int? ModuleId { get; set; }

    }
}
