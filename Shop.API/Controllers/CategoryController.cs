using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.IService;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private ISmsCategoryService smsCategoryService;
        public CategoryController(ISmsCategoryService _smsCategoryService)
        {
            this.smsCategoryService = _smsCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            return new JsonResult(await smsCategoryService.GetAll());
        }
    }
}
