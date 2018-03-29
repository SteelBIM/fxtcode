using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity
{
    /// <summary>
    /// WCF返回消息实体类
    /// </summary>
    public class WCFJsonData
    {
        private int _returntype = 1;
        /// <summary>
        /// 状态:(int)EnumHelper.Status.Success
        /// </summary>
        public int returntype { get { return _returntype; } set { _returntype = value; } } //1为正确
        private string _returntext = "成功";
        /// <summary>
        /// 消息描述：string类型
        /// </summary>
        public string returntext { get { return _returntext; } set { _returntext = value; } }
        private string _data = "";
        /// <summary>
        /// 返回的内容：json字符串
        /// </summary>
        public string data { get { return _data; } set { _data = value; } }
        private string _debug = "";
        /// <summary>
        /// 调试信息
        /// </summary>
        public string debug { get { return _debug; } set { _debug = value; } }
    }

    /// <summary>
    /// sinfo配置文件key实体类
    /// 注意：functionName传的是功能名
    /// author:hody
    /// </summary>
    public class SInfoOfConfigKey
    {
        /// <summary>
        /// appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// config中apppwdKey
        /// </summary>
        public string apppwdKey { get; set; }
        /// <summary>
        /// config中signnameKey
        /// </summary>
        public string signnameKey { get; set; }
        /// <summary>
        /// config中appKey
        /// </summary>
        public string appKey { get; set; }
        /// <summary>
        ///功能名称
        /// </summary>
        public string functionName { get; set; }
        
       /// <summary>
        ///功能名称
        /// </summary>
        public string systypecode { get; set; }

    }
}
