namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 请求对象基础参数
    /// </summary>
    public class BaseRequestEntity
    {

        public BaseRequestEntity()
        {
            DATASOURCE = "WX";

        }


        /// <summary>
        /// 录入人
        /// </summary>
        public string INPUTUSER { get; set; }

        /// <summary>
        /// 业务名ID
        /// </summary>
        public string SALES { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        public string ORGAN { get; set; }

        public string DATASOURCE
        {
            get;
            set;
        }
    }
}
