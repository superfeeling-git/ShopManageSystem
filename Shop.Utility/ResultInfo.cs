using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Utility
{
    public class ResultInfo
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public DateTime Expires { get; set; }
        public string Jwt { get; set; }
    }
}
