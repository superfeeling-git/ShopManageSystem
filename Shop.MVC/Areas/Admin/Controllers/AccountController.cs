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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Hei.Captcha;

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

        private IWebHostEnvironment webHostEnvironment;

        //private SecurityCodeHelper codeHelper = new SecurityCodeHelper();

        public AccountController
        (
            UserManager<SmsUser> _userManager,
            SignInManager<SmsUser> _signInManager,
            ILogger<AccountController> _logger,
            RoleManager<SmsRole> _roleManager,
            IEmailSender _EmailSender,
            IWebHostEnvironment _webHostEnvironment
            //SecurityCodeHelper _codeHelper
        )
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.logger = _logger;
            this.roleManager = _roleManager;
            this.EmailSender = _EmailSender;
            this.webHostEnvironment = _webHostEnvironment;
            //this.codeHelper = _codeHelper;
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
        [HttpGet]
        public IActionResult GenerCode()
        {
            Bitmap bitmap = new Bitmap(6 * 15, 24);

            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);

            ValidateCode validateCode = new ValidateCode();

            Font font = new Font("微软雅黑", 12, FontStyle.Bold | FontStyle.Italic);

            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Red, Color.Blue, 30);

            //SolidBrush brush = new SolidBrush(Color.Red);

            string validatecode = validateCode.GeneralCode();

            Response.Cookies.Append("code", validatecode);

            graphics.DrawString(validatecode, font, linearGradientBrush, rectangle);

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //画线
            for (int i = 0; i < 20; i++)
            {
                graphics.DrawLine(new Pen(Color.FromArgb(100, 0, 0, 255)), random.Next(bitmap.Width), random.Next(bitmap.Height), random.Next(bitmap.Width), random.Next(bitmap.Height));
            }

            MemoryStream memoryStream = new MemoryStream();

            bitmap.Save(memoryStream, ImageFormat.Jpeg);

            //var imgbyte = codeHelper.GetBubbleCodeByte(validatecode);

            return File(memoryStream.ToArray(),"image/jpeg");
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

        public async Task<IActionResult> UploadHeaderAsync(List<IFormFile> file)
        {
            long size = file.Sum(f => f.Length);

            foreach (var formFile in file)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath,"UploadFile",formFile.FileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                        await stream.FlushAsync();
                    }                    
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new {code = 0, count = file.Count, size });
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

    public class ValidateCode
    {
        /// <summary>
        /// 生成随机串
        /// </summary>
        /// <returns></returns>
        public string GeneralCode()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 48; i <= 90; i++)
            {
                if (i < 58 || i > 64)
                    stringBuilder.Append((char)i);
            }

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            string code = stringBuilder.ToString();

            char[] char_code = new char[6];

            for (int i = 0; i < 6; i++)
            {
                char_code[i] = code[random.Next(0, code.Length)];
            }

            return string.Join("", char_code);
        }
    }
}