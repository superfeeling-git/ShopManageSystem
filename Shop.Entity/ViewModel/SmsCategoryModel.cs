using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shop.Entity.ViewModel
{
    public class SmsCategoryModel
    {
        public int value { get; set; }
        public string label { get; set; }
        public List<SmsCategoryModel> children { get; set; }
    }
}
