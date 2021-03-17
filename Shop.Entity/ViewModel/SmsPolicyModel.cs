using System;
using System.Collections.Generic;
using System.Text;
using Shop.Entity.Entity;

namespace Shop.Entity.ViewModel
{
    public class SmsPolicyModel
    {
        public int PolicyID { get; set; }
        public string PolicyName { get; set; }
        public IEnumerable<SmsRole> Roles { get; set; }
    }
}
