using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Shop.API
{
    public class ConfigureAutofac : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            #region 方法1   Load 适用于无接口注入
            var assemblysIRepository = Assembly.Load("Shop.IRepository");

            containerBuilder.RegisterAssemblyTypes(assemblysIRepository)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            var assemblysRepository = Assembly.Load("Shop.Repository");

            containerBuilder.RegisterAssemblyTypes(assemblysRepository)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            var assemblysIService = Assembly.Load("Shop.IService");

            containerBuilder.RegisterAssemblyTypes(assemblysIService)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            var assemblysService = Assembly.Load("Shop.Service");

            containerBuilder.RegisterAssemblyTypes(assemblysService)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            var assemblysAPI = Assembly.Load("Shop.API");

            containerBuilder.RegisterAssemblyTypes(assemblysAPI)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();
            #endregion


            #region 在控制器中使用属性依赖注入，其中注入属性必须标注为public
            //在控制器中使用属性依赖注入，其中注入属性必须标注为public
            var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
                .Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type)).ToArray();
            containerBuilder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
            #endregion
        }
    }
}
