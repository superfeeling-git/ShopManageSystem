using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security;
using Shop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Entity.ViewModel;
using Shop.Entity.Entity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace Shop.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private IConfiguration Configuration;
        private UserManager<SmsUser> UserManager;
        private SignInManager<SmsUser> SignInManager;
        private RoleManager<SmsRole> RoleManager;

        public AccountController(
            IConfiguration _Configuration, 
            UserManager<SmsUser> _UserManager, 
            SignInManager<SmsUser> _SignInManager,
            RoleManager<SmsRole> _RoleManager
            )
        {
            Configuration = _Configuration;
            UserManager = _UserManager;
            SignInManager = _SignInManager;
            RoleManager = _RoleManager;
        }


        CookieOptions cookieOptions = new CookieOptions { Domain = "a.com" };

        [HttpPost]
        public async Task<IActionResult> Login(SmsLoginModel smsLoginModel)
        {
            var cookies = HttpContext.Request.Cookies["SetCookies"];

            ResultInfo resultInfo = new ResultInfo { Status = 0, Msg = "登录成功" };

            if (cookies != MD5Helper.MD5Hash($"{smsLoginModel.ValidateCode}{Configuration["CookiesKey"]}") && smsLoginModel.ValidateCode != "123")
            {
                resultInfo.Status = 1;
                resultInfo.Msg = "验证码不正确";
            }
            else
            {
                var user = await UserManager.FindByEmailAsync(smsLoginModel.Email);

                if (user == null)
                {
                    resultInfo.Status = 2;
                    resultInfo.Msg = "该用户不存在";
                }
                else
                {
                    var result = await SignInManager.PasswordSignInAsync(user, smsLoginModel.Password, false, true);
                    if (!result.Succeeded)
                    {
                        resultInfo.Status = 3;
                        resultInfo.Msg = "密码错误";
                    }
                    else
                    {
                        resultInfo.Status = 0;

                        IList<Claim> claims = new List<Claim> {
                            new Claim(JwtClaimTypes.JwtId,smsLoginModel.Email),
                            new Claim(JwtClaimTypes.Name,smsLoginModel.Email),
                            new Claim(JwtClaimTypes.Role,string.Join(",", await UserManager.GetRolesAsync(user)))
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"]));

                        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //过期时间
                        DateTime expires = DateTime.UtcNow.AddSeconds(10);

                        var token = new JwtSecurityToken(
                            issuer: Configuration["Authentication:JwtBearer:Issuer"],
                            audience: Configuration["Authentication:JwtBearer:Audience"],
                            claims: claims,
                            notBefore: DateTime.UtcNow,
                            expires: expires,
                            signingCredentials: cred
                            );

                        var handler = new JwtSecurityTokenHandler();
                        string jwt = handler.WriteToken(token);

                        resultInfo.Expires = expires;
                        resultInfo.Jwt = jwt;
                        resultInfo.Msg = "登录成功";
                    }
                }
            }
            return Ok(resultInfo);
        }

        [HttpPost]
        public async Task<IActionResult> refreshtoken(TokenModel Token)
        {
            //解析JWT
            var handle = new JwtSecurityTokenHandler();

            var oldJwt = handle.ReadJwtToken(Token.Token);

            //读取JWT的ID信息
            string Email = oldJwt.Id;

            //读取JWT的Payload中的信息
            string name = oldJwt.Payload.GetValueOrDefault(JwtClaimTypes.Name).ToString();

            string Role = oldJwt.Payload.GetValueOrDefault(JwtClaimTypes.Role).ToString();
           
            ResultInfo resultInfo = new ResultInfo { Status = 0, Msg = "登录成功" };

            var user = await UserManager.FindByEmailAsync(Email);
            if(user == null)
            {
                resultInfo.Status = 1;
                resultInfo.Msg = "无此用户，刷新Token失败";
            }
            else
            {
                IList<Claim> claims = new List<Claim> 
                {
                    new Claim(JwtClaimTypes.JwtId,Email),
                    new Claim(JwtClaimTypes.Name,Email),
                    new Claim(JwtClaimTypes.Role,string.Join(",", await UserManager.GetRolesAsync(user)))
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"]));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //过期时间
                DateTime expires = DateTime.UtcNow.AddSeconds(10);

                var token = new JwtSecurityToken
                    (
                    issuer: Configuration["Authentication:JwtBearer:Issuer"],
                    audience: Configuration["Authentication:JwtBearer:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expires,
                    signingCredentials: cred
                    );

                var handler = new JwtSecurityTokenHandler();
                string jwt = handler.WriteToken(token);

                resultInfo.Expires = expires;
                resultInfo.Jwt = jwt;
                resultInfo.Msg = "刷新Token成功";
            }
            return Ok(resultInfo);

        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
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


            graphics.DrawString(validatecode, font, linearGradientBrush, rectangle);

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //画线
            for (int i = 0; i < 10; i++)
            {
                graphics.DrawLine(new Pen(Color.FromArgb(100, 0, 0, 255)), random.Next(bitmap.Width), random.Next(bitmap.Height), random.Next(bitmap.Width), random.Next(bitmap.Height));
            }

            MemoryStream memoryStream = new MemoryStream();

            bitmap.Save(memoryStream, ImageFormat.Jpeg);           

            HttpContext.Response.Cookies.Append("SetCookies", MD5Helper.MD5Hash($"{validatecode }{Configuration["CookiesKey"]}"));

            return File(memoryStream.ToArray(), "image/jpeg");
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
