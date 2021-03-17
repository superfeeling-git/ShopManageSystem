using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using static System.IO.File;

namespace Shop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile GoodsPic)
        {
            _logger.LogInformation($"文件名：{GoodsPic.FileName}");
            _logger.LogInformation($"文件大小：{GoodsPic.Length}");
            _logger.LogInformation($"文件域名：{GoodsPic.Name}");
            _logger.LogInformation($"文件类型：{GoodsPic.ContentType}");

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"{Guid.NewGuid()}{Path.GetExtension(GoodsPic.FileName)}");
            var stream = System.IO.File.Create(filePath);
                GoodsPic.CopyTo(stream);
            stream.Close();
            stream.Dispose();
            
            return View();
        }
    }
}
