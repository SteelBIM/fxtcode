using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 未启用
        /// </summary>
         
        [System.ComponentModel.Description("未启用")]
        NoEnabled =0,
        /// <summary>
        /// 启用
        /// </summary>
        [System.ComponentModel.Description("启用")]
        Enabled = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        [System.ComponentModel.Description("禁用")]
        Disable = 2,
        /// <summary>
        /// 删除
        /// </summary>
        [System.ComponentModel.Description("已删除")]
        Delete = 3
    }
}
