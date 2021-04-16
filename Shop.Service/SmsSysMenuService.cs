using Shop.Entity.Entity;
using Shop.IRepository;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Shop.Entity.ViewModel;

namespace Shop.Service
{
    public class SmsSysMenuService : BaseService<SmsSysMenu, long>, ISmsSysMenuService
    {
        private ISmsSysMenuRepository smsSysMenuRepository;

        public SmsSysMenuService(ISmsSysMenuRepository _smsSysMenuRepository,IUnitOfWork _unitOfWork)
            :base(_smsSysMenuRepository, _unitOfWork)
        {
            this.smsSysMenuRepository = _smsSysMenuRepository;
            this.unitOfWork = _unitOfWork;
        }

        List<TreeModel> treeModels = new List<TreeModel>();

        public async Task<List<TreeModel>> GetAllNodeAsync()
        {
            var MenuList = await smsSysMenuRepository.GetAll();
            foreach (var item in MenuList.Where(m => m.ParentId == 0))
            {
                TreeModel model = new TreeModel { title = item.MenuName, id = item.MenuId, href = item.LinkUrl };

                await SubNodeDataAsync(model,MenuList);

                treeModels.Add(model);
            }

            return treeModels;
        }

        private async Task SubNodeDataAsync(TreeModel model, List<SmsSysMenu> AllList)
        {
            foreach (var item in AllList.Where(m => m.ParentId == model.id))
            {
                TreeModel subNode = new TreeModel { id = item.MenuId, title = item.MenuName, href = item.LinkUrl };

                model.children.Add(subNode);

                await SubNodeDataAsync(subNode, AllList);
            }
        }
    }
}
