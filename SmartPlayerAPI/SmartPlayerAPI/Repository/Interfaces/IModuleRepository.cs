using SmartPlayerAPI.Common;
using SmartPlayerAPI.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Interfaces
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<List<Module>> GetAll();
    }
}
