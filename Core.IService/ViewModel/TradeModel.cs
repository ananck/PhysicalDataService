using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IService.ViewModel
{
    public class TradeModel
    {
        /// <summary>
        /// 制造商ID
        /// </summary>
        public long MaufactureId { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// 牌号
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// 材料标示
        /// </summary>
        public string MaterialIdentity { get; set; }
    }
}
