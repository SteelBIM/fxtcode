using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 用户详细信息表
    /// </summary>
    public class Tb_UserDetails
    {
        public int UserId { get; set; }
        public string AppId { get; set; }
        public string Versions { get; set; }
        /// <summary>
        /// app下载渠道
        /// </summary>
        public int DownloadChannel { get; set; }
        //是否为有效用户 0 不是  1 是
        public int IsValidUser { get; set; }
        /// <summary>
        /// 成为有效用户时间
        /// </summary>
        public DateTime ValidUserTime { get; set; }

    }
}
