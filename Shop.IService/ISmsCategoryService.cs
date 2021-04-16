using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;

namespace Shop.IService
{
    public interface ISmsCategoryService : IBaseService<SmsCategory,long>
    {
        /// <summary>
        /// 获取递归后的ViewModel
        /// </summary>
        /// <returns></returns>
        new Task<List<SmsCategoryModel>> GetAll();
    }
}
