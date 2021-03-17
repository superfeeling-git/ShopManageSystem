using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace Shop.API
{
    public class ShopAOPIntercapter : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            throw new NotImplementedException();
        }
    }
}
