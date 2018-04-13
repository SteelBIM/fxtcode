using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract
{
  
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“KingRequest”的 XML 注释
    public class KingRequest
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“KingRequest”的 XML 注释
    {
        private const string _EncryptKey = "Kingsun.AppLibrary";
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
    }
}
