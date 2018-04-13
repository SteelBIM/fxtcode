using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Models
{
    public class UserModel
    {
        /// <summary>
        /// 服务器Rsa秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 客户端加密信息
        /// </summary>
        public string Info { get; set; }

    }

    public class keyModel
    {
        public string Key { get; set; }
        public string Info { get; set; }
    }
    public class CourseResult
    {
        public string Key { get; set; }
        public string Info { get; set; }
    }

    public class ParmaData
    {
        public string Creator { get; set; }
        /// <summary>
        /// 预留参数，暂时为空
        /// </summary>
        public string MID { get; set; }
        /// <summary>
        /// 预留参数，暂时为空
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 提取码(用户手动输入)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 请求的课程ID
        /// </summary>
        public string CourseID { get; set; }
        /// <summary>
        /// 客户端的公钥
        /// </summary>
        public string PKey { get; set; }
        /// <summary>
        /// 请求配置开始页
        /// </summary>
        public int? Start { get; set; }
        /// <summary>
        /// 请求配置结束页
        /// </summary>
        public int? End { get; set; }
    }

    /// <summary>
    /// 课程部分信息
    /// </summary>
    public class CoursePart
    {
        public string UpdateMD5 { get; set; }
        public string UpdateURL { get; set; }
        public string Version { get; set; }
        public int? TryUpdate { get; set; }
        public string PchKey { get; set; }
    }
}