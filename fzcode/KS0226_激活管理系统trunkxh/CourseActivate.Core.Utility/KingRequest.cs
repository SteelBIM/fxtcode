using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsun.Common
{
    /// <summary>
    /// 服务请求
    /// </summary>
    [Serializable]
    public class KingRequest
    {
        private const string _EncryptKey = "kingsun.AppLibrary";
        /// <summary>
        /// 请求ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string Function
        {
            get;
            set;
        }
        /// <summary>
        /// 业务数据
        /// </summary>
        public string Data
        {
            get;
            set;
        }

        //private UserData _currentUser;

        //public UserData CurrentUser
        //{
        //    get
        //    {
        //        return _currentUser;
        //    }
        //    set
        //    {
        //        _currentUser = value;
        //    }
        //}
        /// <summary>
        /// 解析Cookies值，获得当前用户信息
        /// </summary>
        /// <param name="cookieStr"></param>
        //public void DecodeUser(string cookieStr)
        //{
        //    try
        //    {
        //        DESEncrypt encrypt = new DESEncrypt();
        //        string jsonStr = encrypt.Decrypt(cookieStr, _EncryptKey);
        //        this.CurrentUser = JsonHelper.DecodeJson<UserData>(jsonStr);
        //    }
        //    catch
        //    { 
        //    }
        //}
        /// <summary>
        /// 获取用户信息加密字符串 
        /// </summary>
        /// <returns></returns>
        //public string EncryptUser()
        //{
        //    try
        //    {
        //        string jsonStr = JsonHelper.EncodeJson(this._currentUser);
        //        DESEncrypt encrypt = new DESEncrypt();
        //        jsonStr = encrypt.EncryptToString(jsonStr, _EncryptKey);
        //        string temp = encrypt.Decrypt(jsonStr, _EncryptKey);
        //        return jsonStr;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }

        //}

        /// <summary>
        /// 由字符串反序列化成请求对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static KingRequest DecodeRequest(string json)
        {
            KingRequest request = null;
            try
            {
                request = JsonHelper.DecodeJson<KingRequest>(json);
            }
            catch(Exception e)
            {
               string msg = e.Message;
            }
            return request;
        }
    }
}
