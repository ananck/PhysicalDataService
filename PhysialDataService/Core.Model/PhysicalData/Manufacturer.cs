using System.ComponentModel;

namespace Core.Model
{
    /// <summary>
    /// 制造商表
    /// </summary>
    public partial class Manufacturer : BaseEntity
    {
        /// <summary>
        /// 英文名
        /// </summary>
        [Description("英文名")]
        public string EnglishName { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        [Description("全称")]
        public string FullName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [Description("简称")]
        public string ShortName { get; set; }

    }
}