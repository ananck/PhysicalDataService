﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SaveModel
{
    public class temperaturePropertie
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string propertyName { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// 测试标准
        /// </summary>
        public string testingStandard { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
    }
}
