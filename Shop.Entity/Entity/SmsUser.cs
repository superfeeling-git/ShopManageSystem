using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Shop.Entity.Entity
{
    public class SmsUser : IdentityUser<long>
    {
        public string NickName { get; set; }
        public string HeadPhoto { get; set; }
    }
}
