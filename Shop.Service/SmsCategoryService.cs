using System;
using System.Collections.Generic;
using System.Text;
using Shop.IService;
using Shop.IRepository;
using Shop.Entity.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

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
    }
}
