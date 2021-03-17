using System;
using System.Collections.Generic;
using System.Text;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.IRepository;

namespace Shop.Repository
{
    public class SmsGoodsRepository : BaseRepository<SmsGoods,long>,ISmsGoodsRepository
    {
        public SmsGoodsRepository(SmsDBContext smsDBContext)
            :base(smsDBContext)
        {

        }
    }
}
