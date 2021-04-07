using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface ISmsSysMenuService : IBaseService<SmsSysMenu, long>
    {
        /// <summary>
        /// 获取递归后的所有节点
        /// </summary>
        /// <returns></returns>
        Task<List<TreeModel>> GetAllNodeAsync();
    }
}
