using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Models
{
    /// <summary>
    /// 在线用户列表
    /// </summary>
    public class OnlineUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户登录码
        /// </summary>
        public string UserNum { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录机器唯一码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 登录机器系统
        /// </summary>
        public string MachineModel { get; set; }
    }

    
    public class CatalogueandUser
    {
        public int BookID { get; set; }

        public string UserID { get; set; }

        public int? IsYX { get; set; }
    }
}
