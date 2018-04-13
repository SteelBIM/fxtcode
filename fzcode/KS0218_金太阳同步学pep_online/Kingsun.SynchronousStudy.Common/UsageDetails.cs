using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class UsageDetails
    {
        public string UserId { get; set; }

        /// <summary>
        /// 使用时长 单位分 不足一分按一分计算
        /// </summary>
        public int UsageTimeLength { get; set; }
        public int UsageNumber { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// app版本
        /// </summary>
        public string Versions { get; set; }

        /// <summary>
        /// 下载渠道
        /// </summary>
        public int DownloadChannel { get; set; }
        public DateTime UsageTime { get; set; }
    }
}
