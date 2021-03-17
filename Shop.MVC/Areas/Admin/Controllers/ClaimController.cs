using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using System.Security.Claims;
using System.Reflection;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClaimController : Controller
    {
        private UserManager<SmsUser> userManager;

        public ClaimController(UserManager<SmsUser> _userManager)
        {
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
        /// 添加声明
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(SmsUserClaim smsUserClaim)
        {
            var user = await userManager.FindByIdAsync(smsUserClaim.UserId.ToString());
            var Claim = new Claim(smsUserClaim.ClaimType, smsUserClaim.ClaimValue);
            var result = await userManager.AddClaimAsync(user, Claim);
            if(result.Succeeded)
            {
                return Json(new { code = 0 });
            }
            else
            {
                return Json(new { code = 1 });
            }
        }
    }
}