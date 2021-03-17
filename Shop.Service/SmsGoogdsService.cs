using System;
using System.Collections.Generic;
using System.Text;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.IRepository;
using Shop.IService;

namespace Shop.Service
{
    public class SmsGoogdsService : BaseService<SmsGoods,long>, ISmsGoogdsService
    {
        public SmsGoogdsService(ISmsGoodsRepository smsGoodsRepository, IUnitOfWork unitOfWork)
            :base(smsGoodsRepository,unitOfWork)
        {

        }
    }
}
