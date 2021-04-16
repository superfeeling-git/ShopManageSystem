using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.IService;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Shop.Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GoodsController : ControllerBase
    {
        private ISmsGoogdsService SmsGoogdsService;
        private readonly IWebHostEnvironment hostingEnvironment;


        public GoodsController(ISmsGoogdsService _SmsGoogdsService, IWebHostEnvironment _hostingEnvironment)
        {
            this.SmsGoogdsService = _SmsGoogdsService;
            this.hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// 添加--[FromForm]实现文件Swagger上传
        /// </summary>
        /// <param name="smsGoods"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SmsGoodsModel smsGoods)
        {            
            SmsGoods goods = smsGoods.MapTo<SmsGoods>();
            int result = await SmsGoogdsService.CreateAsync(goods);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> BatchCreateAsync(List<SmsGoods> smsGoods)
        {
            await SmsGoogdsService.BatchCreateAsync(smsGoods);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(SmsGoodsModel smsGoods)
        {
            SmsGoods goods = smsGoods.MapTo<SmsGoods>();
            
            return Ok(await SmsGoogdsService.UpdateAsync(goods));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await SmsGoogdsService.DeleteAsync(id);
            return Ok();
        }

        public class idList
        {
            public long[] id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> BatchDelete(idList id)
        {
            await SmsGoogdsService.BatchDeleteAsync(id.id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await SmsGoogdsService.Find(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await SmsGoogdsService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            return Ok(await SmsGoogdsService.GetList(m => m.GoodsName.Contains("视")));
        }

        [HttpGet]
        public async Task<IActionResult> GetPageAsync(string GoodsName, int? CategoryId, int PageIndex = 1)
        {
            return new JsonResult(await SmsGoogdsService.GetPage(m => m.GoodsId, GoodsName, CategoryId, PageIndex: PageIndex));
        }

        [HttpGet]
        public async Task<IActionResult> IsExist()
        {
            return Ok(await SmsGoogdsService.IsExist(m => m.GoodsName.Contains("视")));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
                await stream.FlushAsync();
            }
            return Ok(new { response="200", file = fileName });
        }

        /// <summary>
        /// CORE中获取路径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPath()
        {
            return Ok(new {
                //通过IWebHostEnvironment获取当前Web根和内容根
                AppName = hostingEnvironment.ApplicationName,
                ContentRootPath =hostingEnvironment.ContentRootPath,
                WebRootPath = hostingEnvironment.WebRootPath,
                EnvironmentName = hostingEnvironment.EnvironmentName,
                //获取当前目录
                dir = Directory.GetCurrentDirectory(),
                //获取DLL所在路径
                appPath= Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath
        });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return new JsonResult(new List<SmsSysMenuModel> { 
                new SmsSysMenuModel
                { 
                    MenuName = "商品管理", 
                    SmsSysMenuModels = new List<SmsSysMenuModel>
                    {
                    new SmsSysMenuModel{MenuName = "商品管理", LinkUrl = "Goods/List"},
                    new SmsSysMenuModel{MenuName = "商品添加", LinkUrl = "Goods/Create"},
                    new SmsSysMenuModel{MenuName = "商品编辑", LinkUrl = "Goods/Edit"},
                    }                
                },
                new SmsSysMenuModel
                {
                    MenuName = "角色管理",
                    SmsSysMenuModels = new List<SmsSysMenuModel>
                    {
                    new SmsSysMenuModel{MenuName = "角色管理", LinkUrl = "Role/List"},
                    new SmsSysMenuModel{MenuName = "角色添加", LinkUrl = "Role/Create"},
                    new SmsSysMenuModel{MenuName = "角色编辑", LinkUrl = "Role/Edit"},
                    }
                }
            });
        }
    }
}
