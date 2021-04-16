using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;

namespace Shop.IService
{
    public interface ISmsGoogdsService : IBaseService<SmsGoods, long>
    {
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        new Task<List<SmsGoodsModel>> GetAll();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        Task<Tuple<List<SmsGoodsModel>, int>> GetPage(Func<SmsGoodsModel, long> keySelector, string GoodsName, int? CategoryId, int PageSize = 10, int PageIndex = 1);
        new Task<SmsGoodsModel> Find(long tkey);
    }
}
