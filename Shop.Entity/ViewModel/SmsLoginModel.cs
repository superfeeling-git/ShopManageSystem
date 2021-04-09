using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop.Entity.ViewModel
{
    public class SmsLoginModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ValidateCode { get; set; }
        public bool Remember { get; set; }        
    }

    public class TokenModel
    {
        public string Token { get; set; }
    }
}
