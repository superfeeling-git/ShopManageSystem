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
            await SmsGoogdsService.CreateAsync(goods);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> BatchCreateAsync(List<SmsGoods> smsGoods)
        {
            await SmsGoogdsService.BatchCreateAsync(smsGoods);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync()
        {
            await SmsGoogdsService.UpdateAsync(m => m.GoodsId == 1, m => new SmsGoods { GoodsName = "品牌电视" });
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await SmsGoogdsService.Find(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await SmsGoogdsService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok(await SmsGoogdsService.GetList(m => m.GoodsName.Contains("视")));
        }

        [HttpGet]
        public async Task<IActionResult> IsExist()
        {
            return Ok(await SmsGoogdsService.IsExist(m => m.GoodsName.Contains("视")));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile)
        {
            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}");
            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
                await stream.FlushAsync();
            }
            return Ok();
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
