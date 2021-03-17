using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Entity.Entity
{
    public class SmsSysMenu
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
        /// 分类深度
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// 分类路径
        /// </summary>
        public string ParentPath { get; set; }
    }
}
