using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Models
{
    public class courseActivatemodelkey
    {
        /// <summary>
        /// 服务器Rsa秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 客户端加密信息
        /// </summary>
        public string Info { get; set; }

        public string CourseID { get; set; }

        public string UserID { get; set; }

    }

    public class CourseActivatKeyResult
    {
        public string Key { get; set; }
        public string Info { get; set; }
    }

    public class CourseActivatKeyInfo
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int Channel { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UName { get; set; }
        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseID { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 是否ios,0为安卓,1为ios
        /// </summary>
        public int isios { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 客户端的公钥
        /// </summary>
        public string PKey { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string RTime { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int? ModularID { get; set; }

    }

    /// <summary>
    /// 课程部分信息
    /// </summary>
    public class SourceInfo
    {
        public string SourceMD5 { get; set; }
        public string SourceURL { get; set; }
        public string SourceVersion { get; set; }
        public string SourceKey { get; set; }
        public int? ActivateNum { get; set; }
        public int? ActivateUseNum { get; set; }
        public string PchKey { get; set; }
        public string Months { get; set; }

        public string ActivateTime { get; set; }

    }

    /// <summary>
    /// 记录错误日志
    /// </summary>
    public class UserIPInfo
    {
        public string IP { get; set; }
        //1 资源链接访问不了 2 资源信息有误
        public int type { get; set; }
    }
}