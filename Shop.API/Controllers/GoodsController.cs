﻿using Microsoft.AspNetCore.Http;
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

namespace Shop.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> CreateAsync([FromForm]SmsGoodsModel smsGoods)
        {
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, $"{Guid.NewGuid()}{Path.GetExtension(smsGoods.GoodsPic.FileName)}");
            using (var stream = System.IO.File.Create(filePath))
            {
                await smsGoods.GoodsPic.CopyToAsync(stream);
            }

            SmsGoods goods = smsGoods.MapTo<SmsGoods>();
            goods.GoodsPic = filePath;
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
            }
            return Ok();
        }
    }
}
