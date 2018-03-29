namespace CAS.Entity.BonaServiceEntity
{
    /// <summary>
    /// 博纳数据接口返回对象
    /// </summary>
    public class BonaReturnDataEntity<T>
    {
        /// <summary>
        /// 错误代码----0为无错误
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string errorMsg { get; set; }

        /// <summary>
        /// 根据业务实际约定返回指定类型的记录总条数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 返回结果集 可能非数组
        /// </summary>
        public T rows { get; set; }
    }


    public class BonaReturnDataEntityForID<T>
    {
        public BonaReturnMessage<T> message { get; set; }

        public string messcode { get; set; }

        public string token { get; set; }

        public string sessionId { get; set; }
    }

    public class BonaReturnMessage<T>
    {
        public string total { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public T rows { get; set; }


    }
}