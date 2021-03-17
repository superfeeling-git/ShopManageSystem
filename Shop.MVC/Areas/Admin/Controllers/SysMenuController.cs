using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SysMenuController : Controller
    {
        [Authorize("/SysMenu/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}