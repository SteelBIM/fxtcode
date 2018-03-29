using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GYSWinform
{
    /// <summary>
    /// 数据缓存实体类
    /// </summary>
    public class Countdown
    {
        /// <summary>
        /// 缓存的key,格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 缓存的数据
        /// </summary>
        public DataEntity data { get; set; }
    }
    /// <summary>
    /// 缓存数据的实体
    /// </summary>
    public class DataEntity : BaseTO
    {
        /// <summary>
        /// 供应商受理业务的ID
        /// </summary>
        public int acceptbusinessid { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 到期时间,格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string expiredate { get; set; }
    }
}
