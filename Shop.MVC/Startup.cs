using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Entity.Entity;
using Shop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Shop.MVC.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Shop.MVC.Mock;
using Microsoft.AspNetCore.Mvc.Controllers;
using Shop.IService;
using Shop.Service;
using Shop.Repository;
using Shop.IRepository;

namespace Shop.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(option => {
                var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();

                option.Filters.Add(new AuthorizeFilter(policy));

                option.Filters.Add<AuthFilter>();
            }).AddRazorRuntimeCompilation();


            services.AddAuthorization(option =>
            {
                //UserManage必须满足以下三个条件
                option.AddPolicy("UserManage", policy=> {
                    //需要该角色
                    policy.RequireRole("Admin", "Super");
                    //需要该声明，有声明即可
                    //policy.RequireClaim("EditUser");
                    //需要该声明，并且值必须正确
                    //policy.RequireClaim("EditUser", "EditUser");
                });

                option.AddPolicy("Custom", policy => {
                    policy.Requirements.Add(new Permission());
                });
            });

            object p = services.AddControllers()
                //添加JSON序列化编码，防止中文被编码为Unicode字符。
                .AddNewtonsoftJson(options => {
                    //修改属性名称的序列化方式，首字母小写
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    //修改时间的序列化方式
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
            });

            //数据库连接
            services.AddDbContext<SmsDBContext>(action => {
                action.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //添加Identity
            services.AddIdentity<SmsUser, SmsRole>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedAccount = true;
                option.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<SmsDBContext>()
                .AddErrorDescriber<CustomIdentityError>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/Account/AccessDenied");
                //options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Admin/Account/Login";
                //ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


            //设置默认的Identity密码强度
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IPolicyData, PolicyData>();

            services.AddTransient<ISmsSysMenuService, SmsSysMenuService>();

            services.AddTransient<ISmsSysMenuRepository, SmsSysMenuRepository>();

            services.AddTransient<IAuthorizationHandler, CustomAuthorize>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IPolicyData policyData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }

    public class Permission : IAuthorizationRequirement
    {

    }

    public class CustomAuthorize : AuthorizationHandler<Permission>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Permission requirement)
        {
            if (context.Resource is Endpoint endpoint)
            {
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            }
            
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            var user = context.HttpContext.User;
        }
    }
}
