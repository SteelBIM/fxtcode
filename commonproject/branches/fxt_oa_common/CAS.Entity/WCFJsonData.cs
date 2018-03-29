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
        /// <summary>
        /// 状态:(int)EnumHelper.Status.Success
        /// </summary>
        public int returntype { get; set; } //1为正确
        /// <summary>
        /// 消息描述：string类型
        /// </summary>
        public string returntext { get; set; }
        /// <summary>
        /// 返回的内容：json字符串
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 调试信息
        /// </summary>
        public string debug { get; set; }
    }
}
