using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private RoleManager<SmsRole> roleManager;
        private UserManager<SmsUser> userManager;

        public RoleController(RoleManager<SmsRole> _roleManager, UserManager<SmsUser> _userManager)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(SmsRoleModel smsRole)
        {
            var result = await roleManager.CreateAsync(new SmsRole {
                 Name = smsRole.RoleName
            });

            if(result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(smsRole.UserId.ToString());

                var addResult = await userManager.AddToRoleAsync(user, smsRole.RoleName);

                if(addResult.Succeeded)
                {
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }
            }
            return Json(new { code = 2 });
        }
    }
}
