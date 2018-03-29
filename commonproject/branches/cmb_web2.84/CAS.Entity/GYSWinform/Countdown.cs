using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// 是否为受理业务：1--受理业务；2--交付业务
        /// </summary>
        public bool isaccept { get; set; }
        /// <summary>
        /// 到期时间,格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string expiredate { get; set; }
    }
}
