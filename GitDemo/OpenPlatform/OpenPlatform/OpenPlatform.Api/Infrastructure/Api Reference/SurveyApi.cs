using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using OpenPlatform.Api.Infrastructure.Utils;
using OpenPlatform.Framework.Utils;

namespace OpenPlatform.Api.Infrastructure
{
    public class SurveyApi
    {
        #region 云查勘接口请求参数
        /// <summary>
        /// 应用ID
        /// </summary>
        private string appId = ConfigurationManager.AppSettings["surveyAppId"];
        /// <summary>
        /// 应用密钥
        /// </summary>
        private string appSecret = ConfigurationManager.AppSettings["surveyAppSecret"];
        /// <summary>
        /// 云查勘服务请求地址
        /// </summary>
        private string surveyPath = ConfigurationManager.AppSettings["surveyPath"];
        /// <summary>
        /// 云查勘服务访问私钥
        /// </summary>
        private string surveySecretPwd = ConfigurationManager.AppSettings["surveySecretPwd"];
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 签名 MD5(appId + loginname + timestamp + 私钥)
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// 请求标识--登录时返回来的
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 请求体
        /// </summary>
        public dynamic body { get; set; }

        public int fxtCompanyId { get; set; }
        /// <summary>
        /// 请求地址需要初始化时重新赋值.前缀已赋值http://192.168.1.29:8080/server/ws/
        /// 只需加上后缀,即各接口对应的URL 如登录为Login 则 UrlApi=UrlApi+SurveyHelper.surveyLogin
        /// 各接口的后缀URL如果有添加需要加如到SurveyHelper.cs类中,方便日后修改
        /// </summary>
        public string UrlApi
        {
            get
            {
                return surveyPath;
            }
            set
            {
                surveyPath = value;
            }
        }

        #region 拼接参数为json格式

        /// <summary>
        /// 将参数转换为请求的json格式 仅登录时需要传入password和appSecret参数,其他时候不用
        /// </summary>
        /// <param name="islogin">访问的api接口是否为登录操作,默认为false,原因是云查勘在登录接口返回了一个token参数, 调用其他接口时必须使用到.只有调登录接口时才不需要传token参数,其他接口都需要</param>
        /// <returns></returns>
        public string GetJsonString(bool islogin = false)
        {
            if (islogin)
            {
                return ToJsonString(new
                {
                    this.appId, 
                    this.appSecret, 
                    this.loginName, 
                    this.password, 
                    this.signature, 
                    this.timestamp, 
                    body
                });
            }
            return ToJsonString(new
            {
                this.appId, 
                this.loginName,
                this.userName, 
                this.signature, 
                this.timestamp, 
                this.token, 
                body
            });
        }

        #endregion

        #region 设置远程服务器证书验证
        /// <summary>
        /// 设置远程服务器证书验证
        /// 用于请求改为 https的阿里云端服务器的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!  
            return true;
        }
        #endregion

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="userContext"></param>
        public SurveyApi(LoginInfoEntity userContext)
        {
            this.loginName = userContext.Username;
            this.password = userContext.PwdToSurvey;
            this.timestamp = GetTimeStamp();
            this.signature = EncryptHelper.GetMd5(appId + loginName + this.timestamp + surveySecretPwd);
            this.token = userContext.Token;
            this.userName = userContext.Truename;
            this.fxtCompanyId = userContext.Fxtcompanyid;
            this.UrlApi = surveyPath;
            //设置远程服务器证书验证
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
        }

        /// <summary>
        /// 将对象转化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToJsonString(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        /// <summary>
        /// 获取时间秒数
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            return (long)(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
        }

        /// <summary>
        /// 获得加密后的密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetPassWordMd5(string pwd)
        {
            string strmd5 = pwd + StringHelper.WcfPassWordMd5Key;
            strmd5 = EncryptHelper.GetMd5(strmd5);
            return strmd5;
        }

        #endregion

        #region 云查勘返回实体类
        /// <summary>
        /// 返回实体类
        /// </summary>
        public class SurveyReturnData<T>
        {
            /// <summary>
            /// body响应体
            /// </summary>
            public T body { get; set; }
            /// <summary>
            /// 返回码
            /// </summary>
            protected int _code = -1;
            public int code
            {
                get { return _code; }
                set { _code = value; }
            }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 服务端签名
            /// </summary>
            public string signature { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string timestamp { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int totalSize { get; set; }
            /// <summary>
            /// 请求标识--登录时返回来的
            /// </summary>
            public string token { get; set; }
        } 
        #endregion

        #region 登陆实体类

        public class LoginInfoEntity 
        {
            public string Username { get; set; }
            public string PwdToSurvey { get; set; }
            public string Token { get; set; }
            public string Truename { get; set; }
            public int Fxtcompanyid { get; set; }
        }

        [Serializable]
        public class Apps
        {
            public string appid { get; set; }
            public string apppwd { get; set; }
            public string appurl { get; set; }
            public string appkey { get; set; }
        }

        /// <summary>
        /// 统一返回的json格式，全部属性小写
        /// </summary>
        [Serializable]
        public class ReturnData
        {
            public int returntype { get; set; } //1为正确
            public object returntext { get; set; }
            public object data { get; set; }
            public object debug { get; set; }
        }

        #endregion
    }
}