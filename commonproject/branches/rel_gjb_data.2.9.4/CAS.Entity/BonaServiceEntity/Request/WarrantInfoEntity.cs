namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 补充权证信息
    /// </summary>
    public class WarrantInfoEntity : BaseRequestEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public string BUSINESSNO { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string CERTTYPE { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FILEURL { get; set; }

        /// <summary>
        /// 房产编号
        /// </summary>
        public string HOUSENO { get; set; }

        /// <summary>
        /// 补充人
        /// </summary>
        public string INPUTUSER { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string UPLOADMAN { get; set; }

    }
}
