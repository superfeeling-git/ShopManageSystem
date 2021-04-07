using System;
using System.Collections.Generic;
using System.Text;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.IRepository;

namespace Shop.Repository
{
    public class SmsSysMenuRepository:BaseRepository<SmsSysMenu,long>, ISmsSysMenuRepository
    {
        public SmsSysMenuRepository(SmsDBContext smsDBContext)
            :base(smsDBContext)
        {

        }
    }
}
