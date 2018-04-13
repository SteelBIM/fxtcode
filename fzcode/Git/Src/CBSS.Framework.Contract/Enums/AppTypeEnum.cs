using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 应用类型
    /// </summary>
    public enum AppTypeEnum
    {
        /// <summary>
        /// 请选择
        /// </summary>
        [System.ComponentModel.Description("请选择")]
        PleaseChoose = -1,
        /// <summary>
        /// 安卓
        /// </summary>
        [System.ComponentModel.Description("安卓")]
        Android = 1,
        /// <summary>
        /// 苹果
        /// </summary>
        [System.ComponentModel.Description("苹果")]
        Ios = 2
    }
}
