using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("UserManage")]
    public class UserController : Controller
    {
        private UserManager<SmsUser> userManager;

        private RoleManager<SmsRole> roleManager;

        public UserController(UserManager<SmsUser> _userManager, RoleManager<SmsRole> _roleManager)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {            
            if(Request.IsAjax())
            {
                int pageSize = 10;
                return Json(new {code = 0,count = userManager.Users.Count(),data= userManager.Users.Skip((page - 1) * pageSize).Take(pageSize) });
            }
            return View();
        }
    }
}
