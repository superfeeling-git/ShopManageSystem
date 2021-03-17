using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Shop.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<SmsUser> userManager;

        private SignInManager<SmsUser> signInManager;

        private RoleManager<SmsRole> roleManager;

        private ILogger<AccountController> logger;

        private IEmailSender EmailSender;

        public AccountController
        (
            UserManager<SmsUser> _userManager,
            SignInManager<SmsUser> _signInManager,
            ILogger<AccountController> _logger,
            RoleManager<SmsRole> _roleManager,
            IEmailSender _EmailSender
        )
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.logger = _logger;
            this.roleManager = _roleManager;
            this.EmailSender = _EmailSender;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(SmsLoginModel smsLoginModel, string ReturnUrl)
        {
            var signInResult = await signInManager.PasswordSignInAsync(
                userName: smsLoginModel.Email, 
                password: smsLoginModel.Password, 
                isPersistent: true, 
                lockoutOnFailure: false);

            if (signInResult.IsLockedOut)
            {
                return Json(new { code = 0, msg = "账户已锁定" });
            }
            if (signInResult.IsNotAllowed)
            {
                return Json(new { code = 0, msg = "不允许此用户登录" });
            }
            if(signInResult.RequiresTwoFactor)
            {
                return Json(new { code = 0, msg = "需要双因子身份验证" });
            }
            if(signInResult.Succeeded)
            {
                if(!string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Json(new { code = 0, msg = "登录成功", returnUrl = ReturnUrl });
                    }
                    else
                    {
                        return Json(new { code = 0, msg = "非本地URL" });
                    }
                }
                else
                {
                    return Json(new { code = 0, msg = "登录成功", returnUrl = ReturnUrl });
                }
            }

            return Json(new { code = 0, msg = "未知错误" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SmsUser smsUser)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(SmsRegisterModel smsRegisterModel,string returnUrl)
        {
            SmsUser smsUser = new SmsUser
            {
                Email = smsRegisterModel.Email,
                UserName = smsRegisterModel.Email
            };
            var result = await userManager.CreateAsync(smsUser, smsRegisterModel.Password);

            if(result.Succeeded)
            {
                //如果需要邮件确认
                if(userManager.Options.SignIn.RequireConfirmedEmail)
                { 

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(smsUser);
                    byte[] bytearr = Encoding.UTF8.GetBytes(code);
                    code = WebEncoders.Base64UrlEncode(bytearr);
                    var callbackUrl = Url.Action
                        ("ConfirmEmail", 
                        "Account", 
                        new { area = "Admin", userId = smsUser.Id, code = code, returnUrl = returnUrl },
                        Request.Scheme);
                    await EmailSender.SendEmailAsync(smsUser.Email, "邮件确认", $"<a href='{callbackUrl}'>点击确认</a>");
                    return Json(new {code = 0,msg = "需要邮件确认" });
                }

                //账户确认
                if (userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return Json(new {code = 0,msg = "需要确认账户" });
                }
                else
                {
                    await signInManager.SignInAsync(smsUser, isPersistent: false);
                    return Json(new { code = 0, msg = "注册成功" });
                }
            }

            string err = string.Empty;
            foreach (var error in result.Errors)
            {
                err += error.Description + ",";
            }
            return Json(new { code = 0, msg = $"错误消息：{err}" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            
            if (Request.IsAjax())
            {
                if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(code))
                {
                    SmsUser smsUser = await userManager.FindByIdAsync(userId);
                    byte[] bytearr = WebEncoders.Base64UrlDecode(code);
                    code = Encoding.UTF8.GetString(bytearr);
                    var result = await userManager.ConfirmEmailAsync(smsUser, code);
                    if (result.Succeeded)
                    {
                        return Json(new { code = 0, msg = "激活成功" });
                    }
                    else
                    {
                        return Json(new { code = 0, msg = result.Errors.Select(m => m.Description) });
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}