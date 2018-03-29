namespace CAS.Entity.DBEntity
{
    public class Dat_EntrustRefund : DatEntrustRefund
    {
        /// <summary>
        /// 退费类型名称
        /// </summary>
        public string RefundTypeName { get; set; }
        /// <summary>
        /// 记录人名
        /// </summary>
        public string RecordPersonName { get; set; }
        /// <summary>
        /// 经手人名
        /// </summary>
        public string HandPersonName { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNo { get; set; }
        /// <summary>
        /// 是否销案  Alex add 2015-09-14
        /// </summary>
        public string closecasedesc { get; set; }

    }
}
