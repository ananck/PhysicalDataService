namespace Core.Model
{

    /// <summary>
    /// 材料商品名称表
    /// </summary>
    public partial class Trade: BaseEntity
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