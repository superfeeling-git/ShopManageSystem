using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Shop.IRepository;
using Microsoft.EntityFrameworkCore;
using Shop.Entity;
using Z.EntityFramework;
using Z.EntityFramework.Plus;
using Z.EntityFramework.Extensions;

namespace Shop.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class,new()
        where TKey : struct
    {
        protected readonly SmsDBContext SmsDBContext;

        public BaseRepository(SmsDBContext _SmsDBContext)
        {
            this.SmsDBContext = _SmsDBContext;
        }

        public async Task BatchCreateAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task BatchDeleteAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().DeleteRangeByKeyAsync(entities);
        }

        public async Task BatchDeleteAsync(TKey[] tkeys)
        {
            await SmsDBContext.Set<TEntity>().DeleteByKeyAsync<TEntity>(tkeys);
        }

        public async Task BatchDeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).DeleteFromQueryAsync();
        }

        public async Task BatchUpdateAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().BulkUpdateAsync(entities);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await SmsDBContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await SmsDBContext.Set<TEntity>().SingleDeleteAsync(entity);
        }

        public async Task DeleteAsync(TKey tkey)
        {
            await SmsDBContext.Set<TEntity>().DeleteByKeyAsync(tkey);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).DeleteFromQueryAsync<TEntity>();
        }

        public async Task<TEntity> Find(TKey tkey)
        {
            return await SmsDBContext.Set<TEntity>().FindAsync(tkey);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await SmsDBContext.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().AnyAsync(expression);
        }

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> exception)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).UpdateFromQueryAsync(exception);
        }
    }
}
