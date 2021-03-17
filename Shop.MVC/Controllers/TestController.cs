using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Shop.MVC.Controllers
{
    [Authorize("Custom")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            string text = "代码改变世界";

            Memory<byte> buffer = Encoding.UTF8.GetBytes(text).AsMemory();
            Response.OnStarting(async () =>
                await Response.Body.WriteAsync(buffer));

            return View();
        }
    }
}
