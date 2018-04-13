using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevelEnum
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        [System.ComponentModel.Description("调试日志")]
        Debug = 1,
        /// <summary>
        /// 系统异常，但不影响系统继续运行的信息
        /// </summary>
        [System.ComponentModel.Description("系统异常")]
        Error = 2,
        /// <summary>
        /// 一般日志,关键系统参数的回显、后台服务的初始化状态、需要开发者确认的信息
        /// </summary>
        [System.ComponentModel.Description("一般日志")]
        Info = 3,
        /// <summary>
        /// 重大错误,影响系统正常运行的信息
        /// </summary>
        [System.ComponentModel.Description("重大错误")]
        Fatal = 100,
        /// <summary>
        /// 一般异常,在业务合理范围内的异常信息
        /// </summary>
        [System.ComponentModel.Description("一般异常")]
        Warn = 4,

    }
}
