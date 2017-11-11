using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPlayerAPI.Persistance;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SmartPlayerAPI.Repository.Persistence
{
    public class PlayerInGameRepository : BaseRepository<PlayerInGame>, IPlayerInGameRepository
    {
        public PlayerInGameRepository(SmartPlayerContext smartPlayerContext) : base(smartPlayerContext)
        {
        }

        public async Task<List<PlayerInGame>> GetListWithInclude(Expression<Func<PlayerInGame, bool>> criteria, Expression<Func<PlayerInGame, object>> columns)
        {
            var result =  _smartPlayerContext
                    .Set<PlayerInGame>()
                    .AsQueryable()
                    .Include(columns)
                    .Where(criteria);
                   // .ConfigureAwait(false);
            return result.ToList();
        }
    }
}
