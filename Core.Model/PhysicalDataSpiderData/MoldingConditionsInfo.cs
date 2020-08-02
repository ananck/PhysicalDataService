using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PhysicalDataSpiderData
{
    /// <summary>
    ///成型条件信息
    /// </summary>
    public class MoldingConditionsInfo
    {
        /// <summary>
        /// 成型条件名称
        /// </summary>
        public string ConditionsInfoName { get; set; }
        /// <summary>
        /// 建议之
        /// </summary>
        public string RecommendedValue { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}
