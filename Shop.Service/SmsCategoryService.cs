using System;
using System.Collections.Generic;
using System.Text;
using Shop.IService;
using Shop.IRepository;
using Shop.Entity.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using Shop.Entity.ViewModel;

namespace Shop.Service
{
    public class SmsCategoryService : BaseService<SmsCategory,long>, ISmsCategoryService
    {
        private ISmsCategoryRepository SmsCategoryRepository;
        public SmsCategoryService(ISmsCategoryRepository _SmsCategoryRepository, IUnitOfWork _unitOfWork)
            :base(_SmsCategoryRepository,_unitOfWork)
        {
            this.SmsCategoryRepository = _SmsCategoryRepository;
            this.unitOfWork = _unitOfWork;
        }

        List<SmsCategoryModel> smsCategories = new List<SmsCategoryModel>();

        public new async Task<List<SmsCategoryModel>> GetAll()
        {
            var list = await SmsCategoryRepository.GetAll();
            foreach (var item in list.Where(m => m.ParentId == 0))
            {
                var model = new SmsCategoryModel
                {
                    value = item.CategoryId,
                    label = item.CategoryName
                };

                if(list.Any(m => m.ParentId == item.CategoryId))
                {
                    model.children = new List<SmsCategoryModel>();
                    await GetChildAsync(list, model);
                }                

                smsCategories.Add(model);
            }

            return smsCategories;
        }

        private async Task GetChildAsync(IList<SmsCategory> allCategories, SmsCategoryModel smsCategoryModel)
        {
            var list = allCategories.Where(m => m.ParentId == smsCategoryModel.value).Select(m => new SmsCategoryModel {
                value = m.CategoryId, label = m.CategoryName
            }).ToList();

            foreach (var item in list)
            {
                if (allCategories.Any(m => m.ParentId == item.value))
                {
                    item.children = new List<SmsCategoryModel>();
                    await GetChildAsync(allCategories, item);
                }
            }

            smsCategoryModel.children.AddRange(list);
        }
    }
}
