namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 记录列表对象
    /// </summary>
    public class RecordEntity : BaseRequestEntity
    {
        /// <summary>
        /// 业务单号
        /// </summary>
        public string SERIALNO { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CUSTOMERNAME { get; set; }

        /// <summary>
        /// 业务类型 A  or D
        /// </summary>
        public string BUSINESSTYPE { get; set; }
    }
}
