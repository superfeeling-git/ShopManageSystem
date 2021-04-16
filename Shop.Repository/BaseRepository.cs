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

        public virtual async Task BatchCreateAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual async Task BatchDeleteAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().DeleteRangeByKeyAsync(entities);
        }

        public virtual async Task BatchDeleteAsync(TKey[] tkeys)
        {
            await SmsDBContext.Set<TEntity>().DeleteByKeyAsync<TEntity>(tkeys);
        }

        public virtual async Task BatchDeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).DeleteFromQueryAsync();
        }

        public virtual async Task BatchUpdateAsync(List<TEntity> entities)
        {
            await SmsDBContext.Set<TEntity>().BulkUpdateAsync(entities);
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await SmsDBContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await SmsDBContext.Set<TEntity>().SingleDeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TKey tkey)
        {
            await SmsDBContext.Set<TEntity>().DeleteByKeyAsync(tkey);
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).DeleteFromQueryAsync<TEntity>();
        }

        public virtual async Task<TEntity> Find(TKey tkey)
        {
            return await SmsDBContext.Set<TEntity>().FindAsync(tkey);
        }

        public virtual async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await SmsDBContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public virtual async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            return await SmsDBContext.Set<TEntity>().AnyAsync(expression);
        }

        public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> exception)
        {
            await SmsDBContext.Set<TEntity>().Where(expression).UpdateFromQueryAsync(exception);
        }

        public virtual void UpdateAsync(TEntity entity)
        {
            SmsDBContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public virtual async Task<Tuple<IList<TEntity>, int>> GetPageAsync(Func<TEntity, TKey> keySelector ,int PageSize = 10,int PageIndex = 1, params Expression<Func<TEntity, bool>>[] where)
        {
            var list = SmsDBContext.Set<TEntity>().AsQueryable();

            foreach (var item in where)
            {
                if(item.Body.NodeType == ExpressionType.Call)
                {
                    MethodCallExpression methodCallExpression = (MethodCallExpression)item.Body;
                    ConstantExpression constantExpression = (ConstantExpression)methodCallExpression.Arguments[0];
                    if(constantExpression.Value != null)
                    {
                        list = list.Where(item);
                    }
                }
                else
                {
                    BinaryExpression expression = item.Body as BinaryExpression;

                    ConstantExpression rightValue = expression.Right as ConstantExpression;

                    if (rightValue.Value != null)
                    {
                        list = list.Where(item);
                    }
                }
            }



            Tuple<IList<TEntity>, int> tuple = new Tuple<IList<TEntity>, int>
            (
            item1: list.OrderBy(keySelector).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList(),
            item2: await list.CountAsync()
            );

            return tuple;
        }
    }
}
