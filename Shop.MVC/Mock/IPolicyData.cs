using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Entity.Entity;
using Shop.Entity.ViewModel;

namespace Shop.MVC.Mock
{
    public interface IPolicyData
    {
        IEnumerable<SmsPolicyModel> getPolicy();
    }
}