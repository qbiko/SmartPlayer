using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPlayerAPI.Persistance;

namespace SmartPlayerAPI.Repository.Persistence
{
    public class PlayerInGameRepository : BaseRepository<PlayerInGame>, IPlayerInGameRepository
    {
        public PlayerInGameRepository(SmartPlayerContext smartPlayerContext) : base(smartPlayerContext)
        {
        }
    }
}
