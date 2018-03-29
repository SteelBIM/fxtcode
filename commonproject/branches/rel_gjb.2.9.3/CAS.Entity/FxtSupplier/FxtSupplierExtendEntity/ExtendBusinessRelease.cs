namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendBusinessRelease : BusinessRelease
    {
        /// <summary>
        /// 供应商简称
        /// </summary>
        public string shortname { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public int businessid { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public decimal? price { get; set; }
         /// <summary>
        /// 业务受理状态:0-拒绝；1-完成；2-未受理;3-任务处理中
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        public int provinceid { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string provincename { get; set; }
    }
}
