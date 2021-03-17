using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Entity.ViewModel
{
    public class SmsSysMenuModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 父分类ID
        /// </summary>
        public int ParentId { get; set; }
    }
}
