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
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 是否左侧显示
        /// </summary>
        public bool IsShowLeft { get; set; }
    }
}
