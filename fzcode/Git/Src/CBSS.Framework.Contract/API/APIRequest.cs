using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract.API
{
    /// <summary>
    /// 服务请求参数
    /// </summary>
    public class APIRequest
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key
        {
            get;
            set;
        }
        /// <summary>
        /// 请求数据
        /// </summary>
        public string Info
        {
            get;
            set;
        }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string FunName
        {
            get;
            set;
        }
        /// <summary>
        /// 接口请求方式，0正常,1加密,2压缩,3压缩加密
        /// </summary>
        public int FunWay
        {
            get;
            set;
        }

        /// <summary>
        /// 接口标识
        /// </summary>
        public string Flag
        {
            get;
            set;
        }    
    }

    /// <summary>
    /// 请求参数标识
    /// </summary>
    public class APIRequestFlag
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 模型ID
        /// </summary>
        public int ModelID
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 请求参数实体
    /// </summary>
    [Serializable]
    public class APIRequestInfo
    {
        /// <summary>
        /// 请求时间
        /// </summary>
        public string RTime { get; set; }

        /// <summary>
        /// 客户端的公钥
        /// </summary>
        public string PKey { get; set; }

        /// <summary>
        /// 输入参数字符串
        /// </summary>
        public string InputStr { get; set; }
    }
}
