using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.IRepository;
using Shop.IService;

namespace Shop.Service
{
    public class SmsGoogdsService : BaseService<SmsGoods,long>, ISmsGoogdsService
    {
        public ISmsGoodsRepository SmsGoodsRepository { get; set; }
        public SmsGoogdsService(ISmsGoodsRepository smsGoodsRepository, IUnitOfWork unitOfWork)
            :base(smsGoodsRepository,unitOfWork)
        {
            this.SmsGoodsRepository = smsGoodsRepository;
        }

        public override Task<int> CreateAsync(SmsGoods entity)
        {
            

            return base.CreateAsync(entity);
        }
    }
}
