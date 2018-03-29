namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 权证是否完整
    /// </summary>
    public class IsBusinessInfoFullEntity
    {
        public string HOUSENO { get; set; }

        public string BUSINESSNO { get; set; }

        public string CERTNAME { get; set; }

        public string CERTTYPE { get; set; }

        /// <summary>
        /// 完整 不完整
        /// </summary>
        public string PASS { get; set; }

    }
}
