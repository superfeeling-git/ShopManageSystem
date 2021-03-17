using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop.Entity.ViewModel
{
    public class SmsRegisterModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
