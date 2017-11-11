using SmartPlayerAPI.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Interfaces
{
    public interface IPlayerInGameRepository : IRepository<PlayerInGame>
    {
        Task<List<PlayerInGame>> GetListWithInclude(Expression<Func<PlayerInGame, bool>> criteria, Expression<Func<PlayerInGame, object>> columns);
    }
}
