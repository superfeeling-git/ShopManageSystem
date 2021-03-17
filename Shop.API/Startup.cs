using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Identity;
using Autofac;
using Microsoft.OpenApi.Models;
using System.IO;

namespace Shop.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string ApiName { get; set; } = "Shop.Core";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddIdentity<SmsUser, SmsRole>()
                .AddEntityFrameworkStores<SmsDBContext>()
                .AddErrorDescriber<CustomIdentityError>();

            //设置默认的Identity密码强度
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} 定义成全局变量，方便修改
                    Version = "V1",
                    Title = $"{ApiName} 接口文档——Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "Shop.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                //就是这里！！！！！！！！！
                var xmlPath = Path.Combine(basePath, "Shop.API.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                //就是这里！！！！！！！！！
                var xmlPath_Entity = Path.Combine(basePath, "Shop.Entity.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath_Entity, true);//默认的第二个参数是false，这个是controller的注释，记得修改
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
