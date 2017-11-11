using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Common;

namespace SmartPlayerAPI.Repository.Persistence
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(SmartPlayerContext smartPlayerContext) : base(smartPlayerContext)
        {
        }

        public async Task<List<Module>> GetAll(int clubId)
        {
            var result =  _dbSet.AsQueryable().Where(i => i.ClubId==clubId).ToList();
            return result;
        }
    }
}
