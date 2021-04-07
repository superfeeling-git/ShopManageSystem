using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using Shop.IService;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class SysMenuController : Controller
    {
        private ISmsSysMenuService SmsSysMenuService;

        public SysMenuController(ISmsSysMenuService _SmsSysMenuService)
        {
            this.SmsSysMenuService = _SmsSysMenuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("get")]
        public async Task<IActionResult> GetNodesAsync()
        {
            return Json(await SmsSysMenuService.GetAllNodeAsync());
        }

        [HttpGet]
        public IActionResult CreateRootNode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNode(SmsSysMenuModel smsSysMenuModel)
        {
            int id = await SmsSysMenuService.CreateAsync(new SmsSysMenu {
                MenuName = smsSysMenuModel.MenuName,
                ParentId = smsSysMenuModel.ParentId, 
                LinkUrl = smsSysMenuModel.LinkUrl, 
                IsShowLeft = smsSysMenuModel.IsShowLeft

            });

            return Ok(new { code = 0, id = id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNode(SmsSysMenuModel smsSysMenuModel)
        {
            await SmsSysMenuService.UpdateAsync(m => m.MenuId == smsSysMenuModel.MenuId, m => new SmsSysMenu {
             MenuName = smsSysMenuModel.MenuName
            });

            return Ok(new { code = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNode(int MenuId)
        {
            await SmsSysMenuService.DeleteAsync(MenuId);

            return Ok(new { code = 0 });
        }
    }
}