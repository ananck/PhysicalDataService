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
        public string EnglishName { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

    }
}