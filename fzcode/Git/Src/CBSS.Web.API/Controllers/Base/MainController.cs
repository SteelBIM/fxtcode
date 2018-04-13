using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Web.API.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CBSS.Web.API.Controllers
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MainController”的 XML 注释
    public class MainController : ApiController
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MainController”的 XML 注释
    {
        /// <summary>
        /// 加密
        /// </summary>
        [HttpPost]
        public APIResponse Encrypt(string funName, object info)
        {
            string keyTmp = "12345678";//Guid.NewGuid().ToString().Substring(0, 8);
           
            //RSA加密后的key,客户端用parmModel.Key进行解密
            string publicKey= ConfigurationManager.AppSettings["PublicKey"];
            publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace(" ", "");
            string keyDes = RSAHelper.encryptData(keyTmp, publicKey, "UTF-8");

            string strData = JsonConvertHelper.ToJson(new { RTime = DateTime.Now, PKey = publicKey, InputStr = info });
            //DES加密返回的data
            string dataDes = SecurityHelper.EncryptDES(strData, keyTmp);
            return APIResponse.GetResponse(new { FunName = funName, Key = keyDes, Info = dataDes, FunWay = 1 });
        }

        /// <summary>
        /// 解密
        /// </summary>
        [HttpPost]
        public APIResponse DecryptReturn(dynamic dy)
        {
            string Key = dy.Key;
            string Info = dy.Info;
            APIData apidate = new APIData();
            APIResponse requestdata = apidate.JudgeRequestData(Key, Info, "Test", 1);
            return requestdata;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        [HttpPost]
        public APIResponse Compress(dynamic dy)
        {
            string data = dy;
            APIData apiData = new APIData();
            var ret = apiData.CompressString(data);
            return APIResponse.GetResponse(ret);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        [HttpPost]
        public APIResponse Decompress(dynamic dy)
        {
            string data = dy;
            APIData apiData = new APIData();
            var ret = apiData.DecompressString(data);
            return APIResponse.GetResponse(ret);
        }
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“KeyInfo”的 XML 注释
    public class KeyInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“KeyInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“KeyInfo.Key”的 XML 注释
        public string Key { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“KeyInfo.Key”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“KeyInfo.Info”的 XML 注释
        public string Info { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“KeyInfo.Info”的 XML 注释
    }
}
