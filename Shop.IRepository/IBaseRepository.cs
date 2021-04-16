using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shop.IRepository
{
    public interface IBaseRepository<TEntity, Tkey>
        where TEntity : class,new()
        where Tkey : struct
    {
        #region 单条添加
        /// <summary>
        /// 单条添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity);
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
        Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> exception);
        #endregion

        #region 更新实体
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        void UpdateAsync(TEntity entity);
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
        Task DeleteAsync(Tkey tkey);
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
        Task BatchDeleteAsync(Tkey[] tkeys);
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
        Task<TEntity> Find(Tkey tkey);
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

        #region 获取分页数据
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<Tuple<IList<TEntity>, int>> GetPageAsync(Func<TEntity, Tkey> keySelector, int PageSize = 10, int PageIndex = 1, params Expression<Func<TEntity, bool>>[] where);
        #endregion
    }
}
