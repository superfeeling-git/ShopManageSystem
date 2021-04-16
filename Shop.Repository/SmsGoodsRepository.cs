using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.IRepository;
using System.Linq;
using Shop.Entity.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Shop.Repository
{
    public class SmsGoodsRepository : BaseRepository<SmsGoods,long>,ISmsGoodsRepository
    {
        public SmsGoodsRepository(SmsDBContext smsDBContext)
            :base(smsDBContext)
        {

        }

        public new async Task<List<SmsGoodsModel>> GetAll()
        {
            return await SmsDBContext.SmsCategory.Join
                (SmsDBContext.SmsGoods, a => a.CategoryId, b => b.CategoryId, 
                (a, b) => new SmsGoodsModel {
                    AddTime = b.AddTime,
                    CategoryId = b.CategoryId,
                    CategoryName = a.CategoryName,
                    GoodsId = b.GoodsId,
                    GoodsName = b.GoodsName,
                    GoodsPic = b.GoodsPic,
                    GoodsPrice = b.GoodsPrice
                }).ToListAsync();
        }

        public async Task<Tuple<List<SmsGoodsModel>, int>> GetPageAsync(Func<SmsGoodsModel, long> keySelector, string GoodsName, int? CategoryId, int PageSize, int PageIndex)
        {
            var list = SmsDBContext.SmsCategory.Join
                (SmsDBContext.SmsGoods, a => a.CategoryId, b => b.CategoryId,
                (a, b) => new SmsGoodsModel
                {
                    AddTime = b.AddTime,
                    CategoryId = b.CategoryId,
                    CategoryName = a.CategoryName,
                    GoodsId = b.GoodsId,
                    GoodsName = b.GoodsName,
                    GoodsPic = b.GoodsPic,
                    GoodsPrice = b.GoodsPrice
                }).AsQueryable();
            if (!string.IsNullOrWhiteSpace(GoodsName))
            {
                list = list.Where(m => m.GoodsName.Contains(GoodsName));
            }

            if(CategoryId != null)
            {
                list = list.Where(m => m.CategoryId == CategoryId);
            }

            Tuple<List<SmsGoodsModel>, int> tuple = new Tuple<List<SmsGoodsModel>, int>(
                item1: list.OrderBy(keySelector).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList(),
                item2: await list.CountAsync()
                );

            return tuple;
        }

        public new async Task<SmsGoodsModel> Find(long tkey)
        {
            var list = await SmsDBContext.SmsCategory.Join
                (SmsDBContext.SmsGoods.Where(m => m.GoodsId == tkey), a => a.CategoryId, b => b.CategoryId, (a, b) => new { a, b }
                ).ToListAsync();

            return list.Select(m => new SmsGoodsModel
            {
                AddTime = m.b.AddTime,
                CategoryId = m.b.CategoryId,
                CategoryName = m.a.CategoryName,
                GoodsName = m.b.GoodsName,
                Content = m.b.Content,
                GoodsId = m.b.GoodsId,
                GoodsPic = m.b.GoodsPic,
                GoodsPrice = m.b.GoodsPrice,
                CategoryPath =
                Array.ConvertAll($"{m.a.ParentPath},{m.b.CategoryId}".Split(','), m => Convert.ToInt32(m))
                .Select((m, i) => new { m, i }).Where(m => m.i > 0).Select(m => m.m).ToArray()
            }).FirstOrDefault();
        }
    }
}
