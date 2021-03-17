using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Entity.ViewModel
{
    public class SmsGoodsModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public IFormFile GoodsPic { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
    }
}
