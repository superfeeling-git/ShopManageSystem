using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Shop.Entity;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using Shop.IRepository;
using Shop.IService;
using System.IO;
using System.Linq;

namespace Shop.Service
{
    public class SmsGoogdsService : BaseService<SmsGoods,long>, ISmsGoogdsService
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public ISmsGoodsRepository SmsGoodsRepository { get; set; }
        public SmsGoogdsService(ISmsGoodsRepository smsGoodsRepository, IUnitOfWork unitOfWork, IWebHostEnvironment _hostingEnvironment)
            :base(smsGoodsRepository,unitOfWork)
        {
            this.SmsGoodsRepository = smsGoodsRepository;
            this.hostingEnvironment = _hostingEnvironment;
        }

        public override Task<int> CreateAsync(SmsGoods entity)
        {
            return base.CreateAsync(entity);
        }

        public new async Task<List<SmsGoodsModel>> GetAll()
        {
            return await SmsGoodsRepository.GetAll();
        }

        public async Task<Tuple<List<SmsGoodsModel>, int>> GetPage(Func<SmsGoodsModel, long> keySelector, string GoodsName, int? CategoryId, int PageSize = 10, int PageIndex = 1)
        {
            return await SmsGoodsRepository.GetPageAsync(keySelector, GoodsName, CategoryId, PageSize, PageIndex);
        }

        public async new Task<SmsGoodsModel> Find(long tkey)
        {
            return await SmsGoodsRepository.Find(tkey);
        }

        public async override Task DeleteAsync(long tkey)
        {
            var model = await Find(tkey);

            var picPath = $"{hostingEnvironment.WebRootPath}/{model.GoodsPic}";

            //删除图片
            if(File.Exists(picPath))
            {
                File.Delete(picPath);
            }

            await base.DeleteAsync(tkey);
        }

        public async override Task BatchDeleteAsync(long[] tkeys)
        {
            var list = await GetList(m => tkeys.Contains(m.GoodsId));
            foreach (var item in list)
            {
                var picPath = $"{hostingEnvironment.WebRootPath}/{item.GoodsPic}";

                //删除图片
                if (File.Exists(picPath))
                {
                    File.Delete(picPath);
                }
            }
            await base.BatchDeleteAsync(m => tkeys.Contains(m.GoodsId));
        }
    }
}
