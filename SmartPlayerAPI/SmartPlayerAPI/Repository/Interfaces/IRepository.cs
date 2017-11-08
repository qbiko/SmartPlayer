using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Repository.Interfaces
{
    public interface IRepository<TAggregate>
    {
        Task<TAggregate> AddAsync(TAggregate item);
        Task<TAggregate> Update(TAggregate item);
        Task<bool> Delete(TAggregate item);
        Task<TAggregate> FindById(int id);
        Task<TAggregate> FindByCriteria(Expression<Func<TAggregate, bool>> criteria);
        Task<TAggregate> FindWithInclude(Expression<Func<TAggregate, bool>> criteria, Expression<Func<TAggregate, object>> columns);

    }
}
