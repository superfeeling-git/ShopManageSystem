using Shop.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Entity.ViewModel;

namespace Shop.MVC.Mock
{
    public class PolicyData : IPolicyData
    {
        public IEnumerable<SmsPolicyModel> getPolicy()
        {
            List<SmsPolicy> policies = new List<SmsPolicy> {
                new SmsPolicy{ PolicyID = 1, PolicyName = "UserManage" },
                new SmsPolicy{ PolicyID = 2, PolicyName = "ProductManage" },
                new SmsPolicy{ PolicyID = 3, PolicyName = "OrderManage" },
                new SmsPolicy{ PolicyID = 4, PolicyName = "SuperManage" },
            };

            List<SmsRole> smsRoles = new List<SmsRole> {
                new SmsRole{ Id = 1, Name = "CreateUser" },
                new SmsRole{ Id = 1, Name = "EditUser" },
                new SmsRole{ Id = 1, Name = "ManageUser" },
                new SmsRole{ Id = 1, Name = "DeleteUser" },
            };

            List<SmsPolicyRole> policyRoles = new List<SmsPolicyRole> { 
                new SmsPolicyRole{ ID = 1, PolicyID = 1, RoleID = 1 },
                new SmsPolicyRole{ ID = 2, PolicyID = 1, RoleID = 2 },
                new SmsPolicyRole{ ID = 3, PolicyID = 1, RoleID = 3 },
                new SmsPolicyRole{ ID = 4, PolicyID = 1, RoleID = 4 }
            };


            List<SmsPolicyModel> smsPolicyModels = new List<SmsPolicyModel>();
            foreach (var item in policies)
            {
                var model = new SmsPolicyModel { PolicyID = item.PolicyID, PolicyName = item.PolicyName };

                model.Roles = smsRoles.Join(policyRoles, 
                                a => a.Id, 
                                b => b.RoleID, 
                                (a, b) => new { Id = a.Id,Name = a.Name, PolicyId = b.PolicyID })
                                .Where(m=>m.PolicyId == item.PolicyID)
                                .Select(m=>new SmsRole { Id = m.Id, Name = m.Name });

                smsPolicyModels.Add(model);
            }

            return smsPolicyModels;
        }
    }
}
