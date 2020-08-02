using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PhysicalDataSpiderData
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// 材料标示
        /// </summary>
        public string MaterialIdentification { get; set; }
        /// <summary>
        /// UL档案号
        /// </summary>
        public string ULFileNumber { get; set; }
        /// <summary>
        /// 其他证书
        /// </summary>
        public string OtherCertificates { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// 材料特性
        /// </summary>
        public string MaterialProperties { get; set; }
        /// <summary>
        /// 材料形状
        /// </summary>
        public string MaterialShape { get; set; }
    }
}
