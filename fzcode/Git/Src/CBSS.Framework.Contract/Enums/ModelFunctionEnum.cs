using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 模型功能
    /// </summary>
    public enum ModelFunctionEnum
    {

        /// <summary>
        /// 评测
        /// </summary> 
        [System.ComponentModel.Description("评测")]
        Evaluating = 1,
        /// <summary>
        /// 跟读
        /// </summary> 
        [System.ComponentModel.Description("跟读")]
        Repeat = 2
    }
}
