﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PhysicalDataSpiderData
{
    /// <summary>
    /// 牌号信息
    /// </summary>
    public class SpiderDataInfo
    {
        public long maufactureId;

        /// <summary>
        /// 简称
        /// </summary>
        public string MakerShortName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string MakerEnglishName { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        public string MakerFullName { get; set; }
        /// <summary>
        /// 商品种类
        /// </summary>
        public string CommodityClass { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string TradeName { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 牌号
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// 档案号
        /// </summary>
        public string FileNumber { get; set; }
        /// <summary>
        /// 材料标识
        /// </summary>
        public string MaterialIdentity { get; set; }
        
        /// <summary>
        /// 产品信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }
        /// <summary>
        /// 物性信息列表
        /// </summary>
        public Dictionary<string, List<PhysicalInfo>> PhysicalInfos { get; set; } = new Dictionary<string, List<PhysicalInfo>>();
        /// <summary>
        /// 成型信息列表
        /// </summary>
        public Dictionary<string, List<MoldingConditionsInfo>> MoldingConditionsInfos { get; set; } = new Dictionary<string, List<MoldingConditionsInfo>>();
        /// <summary>
        /// 材料特性
        /// </summary>
        public string Characteristic { get; set; }
        /// <summary>
        /// 材料形状
        /// </summary>
        public string Shape { get; set; }
        /// <summary>
        /// 加工方法
        /// </summary>
        public string ProcessingMethod { get; set; }
        /// <summary>
        /// 详情URL
        /// </summary>
        public string DetailsUrl { get; set; }
    }
}
