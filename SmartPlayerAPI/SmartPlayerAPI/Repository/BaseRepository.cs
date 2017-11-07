using SmartPlayerAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SmartPlayerAPI.Persistance;
using Microsoft.EntityFrameworkCore;
using SmartPlayerAPI.Common;

namespace SmartPlayerAPI.Repository
{
    public abstract class BaseRepository<TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregate
    {
        public SmartPlayerContext _smartPlayerContext;
        public readonly DbSet<TAggregate> _dbSet;

        public BaseRepository(SmartPlayerContext smartPlayerContext)
        {
            _smartPlayerContext = smartPlayerContext;
            _dbSet = _smartPlayerContext.Set<TAggregate>();
        }

        public async Task<TAggregate> AddAsync(TAggregate item)
        {
            var result = _dbSet.Add(item)?.Entity;
            if (result != null)
                await _smartPlayerContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> Delete(TAggregate item)
        {
            bool result = (_dbSet.Remove(item)?.Entity != null);
            if (result != false)
                await _smartPlayerContext.SaveChangesAsync();
            return result;
        }

        public async Task<TAggregate> FindById(int id)
        {
            var result = await _dbSet.AsQueryable().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            return result;
        }

        public async Task<TAggregate> Update(TAggregate item)
        {
            var result = _dbSet.Update(item)?.Entity;
            if (result != null)
                await _smartPlayerContext.SaveChangesAsync();
            return result;
        }

        public async Task<IList<TAggregate>> FindByCriteria(Expression<Func<TAggregate, bool>> criteria)
        {
            var result = await _dbSet.AsQueryable().Where(criteria).ToListAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<TAggregate> FindWithInclude(Expression<Func<TAggregate, bool>> criteria, Expression<Func<TAggregate, object>> columns)
        {
            var result = await _smartPlayerContext
                    .Set<TAggregate>()
                    .AsQueryable()
                    .Include(columns)
                    .SingleOrDefaultAsync(criteria)
                    .ConfigureAwait(false);
            return result;
        }
    }
}
