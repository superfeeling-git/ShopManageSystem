using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IBaseService<TEntity,TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        #region 单条添加
        /// <summary>
        /// 单条添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> CreateAsync(TEntity entity);
        #endregion

        #region 批量添加
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchCreateAsync(List<TEntity> entities);
        #endregion

        #region 按查询条件更新
        /// <summary>
        /// 按查询条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> expression1);
        #endregion

        #region 更新实体
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        Task<int> UpdateAsync(TEntity entity);
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchUpdateAsync(List<TEntity> entities);
        #endregion

        #region 按实体删除
        /// <summary>
        /// 按实体删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);
        #endregion

        #region 按主键删除
        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="tkey"></param>
        /// <returns></returns>
        Task DeleteAsync(TKey tkey);
        #endregion

        #region 按条件删除
        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        #endregion

        #region 批量按实体删除
        /// <summary>
        /// 批量按实体删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(List<TEntity> entities);
        #endregion

        #region 批量按主键删除
        /// <summary>
        /// 批量按主键删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(TKey[] tkeys);
        #endregion

        #region 批量按查询条件删除
        /// <summary>
        /// 批量按查询条件删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(Expression<Func<TEntity, bool>> expression);
        #endregion

        #region 获取单条数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="tkey"></param>
        /// <returns></returns>
        Task<TEntity> Find(TKey tkey);
        #endregion

        #region 按照条件获取实体
        /// <summary>
        /// 按照条件获取实体
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        #endregion

        #region 获取所有实体
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAll();
        #endregion       

        #region 根据条件获得集合
        /// <summary>
        /// 根据条件获得集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> expression);
        #endregion

        #region 根据条件查询是否存在
        /// <summary>
        /// 根据条件查询是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
        #endregion
    }
}
