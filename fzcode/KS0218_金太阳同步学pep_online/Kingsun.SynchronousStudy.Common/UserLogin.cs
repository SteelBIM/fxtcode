using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class UserLogin
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// app版本
        /// </summary>
        public string Versions { get; set; }

        /// <summary>
        /// 下载渠道
        /// </summary>
        public int DownloadChannel { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 0 登录 1注册
        /// </summary>
        public int LoginType { get; set; }
    }
}
