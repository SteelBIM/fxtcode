using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtUserCenterService.Entity
{
    /// <summary>
    /// 获取用户信息与安全信息
    /// </summary>
    public class UserAndAppInfo
    {
        public Security sinfo { get; set; }
        public CUserInfo uinfo { get; set; }
    }
    /// <summary>
    /// 安全信息
    /// </summary>
    public class Security 
    {
        ///// <summary>
        ///// appi
        ///// </summary>
        //public int appid { get; set; }
        ///// <summary>
        ///// apppwd
        ///// </summary>
        //public string apppwd { get; set; }
        ///// <summary>
        /// signname
        /// </summary>
        public string signname { get; set; }

        /// <summary>
        /// apiurl
        /// </summary>
        //public string appurl { get; set; }

        /// <summary>
        /// businessdb
        /// </summary>
        public string businessdb { get; set; }

        /// <summary>
        /// appkey
        /// </summary>
        //public string appkey { get; set; }


        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// producttypecode
        /// </summary>
        public int producttypecode { get; set; }

        public string weburl { get; set; }

        public List<Apps> apps { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CUserInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname { get; set; }

        /// <summary>
        /// 分支机构Id
        /// </summary>
        public string subcompanyid { get; set; }

        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string subcompany { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 修改账号对应密码跳转地址
        /// </summary>
        public string editpwdurl { get; set; }

        /// <summary>
        /// 检测为简单密码提示
        /// </summary>
        public string returntext { get; set; }
    }

    /// <summary>
    /// apps
    /// </summary>
    public class Apps
    {
         /// <summary>
        /// appi
        /// </summary>
        public int appid { get; set; }
        /// <summary>
        /// apppwd
        /// </summary>
        public string apppwd { get; set; }

        /// <summary>
        /// apiurl
        /// </summary>
        public string appurl { get; set; }

        /// <summary>
        /// appkey
        /// </summary>
        public string appkey { get; set; }


    }
}
