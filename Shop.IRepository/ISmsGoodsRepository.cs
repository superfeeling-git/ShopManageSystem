using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;

namespace Shop.IRepository
{
    public interface ISmsGoodsRepository : IBaseRepository<SmsGoods, long>
    {
        new Task<List<SmsGoodsModel>> GetAll();
        #region 获取分页数据
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<Tuple<List<SmsGoodsModel>, int>> GetPageAsync(Func<SmsGoodsModel, long> keySelector, string GoodsName, int? CategoryId, int PageSize = 10, int PageIndex = 1);
        #endregion
        new Task<SmsGoodsModel> Find(long tkey);
    }
}
