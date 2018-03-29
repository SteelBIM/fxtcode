using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    /// <summary>
    /// 用户检查类，中心API返回的实体 kevin 2013-4-2
    /// </summary>
    public class UserCheck : BaseTO
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户有效
        /// </summary>
        public int uservalid { get; set; }
        /// <summary>
        /// 机构有效
        /// </summary>
        public int companyvalid { get; set; }
        /// <summary>
        /// 产品有效
        /// </summary>
        public int productvalid { get; set; }
        /// <summary>
        /// 业务数据库连接
        /// </summary>
        public string businessdb { get; set; }
        /// <summary>
        /// 产品版本
        /// </summary>
        public string currentversion { get; set; }
        /// <summary>
        /// 产品生效日期
        /// </summary>
        public DateTime startdate { get; set; }
        /// <summary>
        /// 产品到期日期
        /// </summary>
        public DateTime overdate { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public int companyid { get; set; }
        /// <summary>
        /// 机构代码，用户后缀
        /// </summary>
        public string companycode { get; set; }
        public string companyname { get; set; }
        /// <summary>
        /// 产品CODE 1003
        /// </summary>
        public int producttypecode { get; set; }
        /// <summary>
        /// 产品Web url
        /// </summary>
        public string weburl { get; set; }
        /// <summary>
        /// 产品Api ，如云查勘api
        /// </summary>
        public string apiurl { get; set; }
        /// <summary>
        /// 产品对外Api
        /// </summary>
        public string outapiurl { get; set; }
        /// <summary>
        /// 消息服务器
        /// </summary>
        public string msgserver { get; set; }

        /// <summary>
        /// 短信发送者
        /// </summary>
        public string smssendname { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string wxname { get; set; }

        ///// <summary>
        ///// 密码
        ///// </summary>
        //public string userpwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string emailstr { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string wxopenid { get; set; }

        /// <summary>
        /// 真实名
        /// </summary>
        public string truename { get; set; }
               
         /// <summary>
        /// 公司标识
        /// </summary>
        public string signname { get; set; }
        /// <summary>
        /// 商户（应用）ID
        /// </summary>
        public string applicationid { get; set; }
        /// <summary>
        /// 商户（应用）密码
        /// </summary>
        public string applicationpwd { get; set; }
        /// <summary>
        /// 约定的加密密钥，不出现在URL中
        /// </summary>
        public string encryptionkey { get; set; }

        public string appid { get; set; }
        public string apppwd { get; set; }
        public string appurl { get; set; }
        public string appkey { get; set; }
    }
}
