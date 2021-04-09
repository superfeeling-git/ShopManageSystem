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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Filters;

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
            //���JSON���л����룬��ֹ���ı�����ΪUnicode�ַ���
            .AddNewtonsoftJson(options => {
                //�޸��������Ƶ����л���ʽ������ĸСд
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                //�޸�ʱ������л���ʽ
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
            });
              

            services.AddDistributedMemoryCache();

            //���ݿ�����
            services.AddDbContext<SmsDBContext>(action => {
                action.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddCors(option =>
            {
                option.AddDefaultPolicy(policy =>
                {
                    //��Ӧ�ͻ���withCredentials����Ҫ���þ������������
                    policy.WithOrigins("http://web.a.com:81").AllowCredentials();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });

            //���Identity
            services.AddIdentity<SmsUser, SmsRole>()
                .AddEntityFrameworkStores<SmsDBContext>()
                .AddErrorDescriber<CustomIdentityError>();

            //����Ĭ�ϵ�Identity����ǿ��
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                option => {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        //�Ƿ���֤������
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],//������

                        //�Ƿ���֤������
                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:JwtBearer:Audience"],//������

                        //�Ƿ���֤��Կ
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"])),

                        ValidateLifetime = true, //��֤��������

                        RequireExpirationTime = true, //����ʱ��

                        ClockSkew = TimeSpan.Zero   //ƽ������ƫ��ʱ��
                    };
                }
            );


            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} �����ȫ�ֱ����������޸�
                    Version = "V1",
                    Title = $"{ApiName} �ӿ��ĵ�����Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "Shop.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                });
                options.OrderActionsBy(o => o.RelativePath);

                //�����������������������
                var xmlPath = Path.Combine(basePath, "Shop.API.xml");//������Ǹո����õ�xml�ļ���
                options.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                //�����������������������
                var xmlPath_Entity = Path.Combine(basePath, "Shop.Entity.xml");//������Ǹո����õ�xml�ļ���
                options.IncludeXmlComments(xmlPath_Entity, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                //����Ȩ��С��
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //��header�����token�����ݵ���̨
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���)ֱ���������������Bearer {token}(ע������֮����һ���ո�) \"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
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

                //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
