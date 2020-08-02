using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PhysicalDataSpiderData
{
    /// <summary>
    /// 物性信息
    /// </summary>
    public class PhysicalInfo
    {
        /// <summary>
        /// 物性类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// 测试标准
        /// </summary>
        public string TestStandard { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Company { get; set; }
    }
}
