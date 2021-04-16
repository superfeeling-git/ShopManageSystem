using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shop.IService;
using Shop.IRepository;
using Shop.Utility;

namespace Shop.Service
{
    public abstract class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey>
        where TEntity : class,new()
        where TKey : struct
    {
        protected IBaseRepository<TEntity, TKey> baseRepository;
        protected IUnitOfWork unitOfWork;

        public BaseService(IBaseRepository<TEntity, TKey> _baseRepository, IUnitOfWork _unitOfWork)
        {
            this.baseRepository = _baseRepository;
            this.unitOfWork = _unitOfWork;
        }

        public virtual async Task BatchCreateAsync(List<TEntity> entities)
        {
            await baseRepository.BatchCreateAsync(entities);
            await unitOfWork.SaveChangeAsync();            
        }

        public virtual async Task BatchDeleteAsync(List<TEntity> entities)
        {
            await baseRepository.BatchDeleteAsync(entities);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task BatchDeleteAsync(TKey[] tkeys)
        {
            await baseRepository.BatchDeleteAsync(tkeys);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task BatchDeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await baseRepository.BatchDeleteAsync(expression);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task BatchUpdateAsync(List<TEntity> entities)
        {
            await baseRepository.BatchUpdateAsync(entities);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            await baseRepository.CreateAsync(entity);
            return await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await baseRepository.DeleteAsync(entity);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task DeleteAsync(TKey tkey)
        {
            await baseRepository.DeleteAsync(tkey);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await baseRepository.DeleteAsync(expression);
            await unitOfWork.SaveChangeAsync();
        }

        public virtual async Task<TEntity> Find(TKey tkey)
        {
            return await baseRepository.Find(tkey); 
        }

        public virtual async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await baseRepository.Find(expression);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await baseRepository.GetAll();
        }

        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return await baseRepository.GetList(expression);
        }

        public virtual async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            return await baseRepository.IsExist(expression);
        }

        public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> exception)
        {
            await baseRepository.UpdateAsync(expression, exception);
            await unitOfWork.SaveChangeAsync();
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            baseRepository.UpdateAsync(entity);
            return await unitOfWork.SaveChangeAsync();
        }
    }
}
