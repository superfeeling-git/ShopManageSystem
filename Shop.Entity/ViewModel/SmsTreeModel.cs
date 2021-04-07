using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Entity.ViewModel
{
    public class TreeModel
    {
        public string title { get; set; }
        public int id { get; set; }
        public List<TreeModel> children { get; set; } = new List<TreeModel>();
        public bool spread { get; set; } = true;
        public string href { get; set; }
    }
}
