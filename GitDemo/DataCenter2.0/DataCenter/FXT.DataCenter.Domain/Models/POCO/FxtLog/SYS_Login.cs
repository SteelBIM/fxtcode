using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 登录 退出日志
    /// </summary>
    public class SYS_Login
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        public int FxtCompanyId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime LogOutDate { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 32位 唯一识别码
        /// </summary>
        public string PasCdoe { get; set; }

        /// <summary>
        /// 系统类型
        /// （估价宝、云查勘。。。。。。。）
        /// </summary>
        public int SysTypeCode { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string BrowserType { get; set; }


    }
}
