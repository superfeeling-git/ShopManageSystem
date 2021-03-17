using System;
using System.Collections.Generic;
using System.Text;
using Shop.Entity.Entity;
using Shop.IRepository;
using Shop.Entity;

namespace Shop.Repository
{
    public class SmsCategoryRepository : BaseRepository<SmsCategory,long>, ISmsCategoryRepository
    {
        public SmsCategoryRepository(SmsDBContext _SmsDBContext)
            : base(_SmsDBContext)
        {
        }
    }
}
