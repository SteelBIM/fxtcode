using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBSS.Web.API.Common
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LogVO”的 XML 注释
    public class LogVO
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LogVO”的 XML 注释
    {
        /// <summary>
        /// 契约名称
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 输入（序列化成json字符串）
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 输出（序列化成json字符串）
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 耗时(ms)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

    }
}