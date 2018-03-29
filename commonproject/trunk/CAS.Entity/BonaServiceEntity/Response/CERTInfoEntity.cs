namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 权证信息
    /// </summary>
    public class CERTInfoEntity
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string ISPASS { get; set; }

        /// <summary>
        /// image url
        /// </summary>
        public string FILEURL { get; set; }

        public string SERIALNO { get; set; }

        /// <summary>
        /// 房产编号
        /// </summary>
        public string HOUSENO { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        public string BUSINESSNO { get; set; }

        public string CERTNAME { get; set; }

        public string CERTTYPE { get; set; }

    }
}
