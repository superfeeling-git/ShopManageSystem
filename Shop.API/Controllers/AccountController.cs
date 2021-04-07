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

namespace Shop.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        CookieOptions cookieOptions = new CookieOptions { Domain = "a.com" };

        [HttpGet]
        public IActionResult Login()
        {
            string token = MD5Helper.MD5Hash(Guid.NewGuid().ToString());

            //HttpContext.Request.Headers.Add("Access-Control-Allow-Origin", "http://web.a.com:81/");

            HttpContext.Response.Cookies.Append("PostCookies", Guid.NewGuid().ToString());

            return Ok();
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

            HttpContext.Response.Cookies.Append("SetCookies", Guid.NewGuid().ToString());

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
