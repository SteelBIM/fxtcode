using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Models
{
    public class courseActivatemodel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 激活码
        /// </summary>
        public string activatecode { get; set; }
        /// <summary>
        /// 设备编号 1-pc 2-mobile
        /// </summary>
        public int? devicetype { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string devicecode { get; set; }
        /// <summary>
        /// 课本编号
        /// </summary>
        public int? bookid { get; set; }
    }
}