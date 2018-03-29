namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 服务记录查寻对象
    /// </summary>
    public class RecordLisSearchEntity : BaseRequestEntity
    {
        public string SALES { get; set; }

        public string CUSTOMERNAME { get; set; }

        public string CUSTOMERIDNUM { get; set; }
        public string BUSINESSTYPE { get; set; }
        /// <summary>
        /// 金融进度
        /// </summary>
        public string CERTPROGRESS { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public string AUDITSTATUS { get; set; }

    }
}
