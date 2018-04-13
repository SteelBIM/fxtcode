using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 试看条件
    /// </summary>
    public enum FreeTypeEnum
    {

        /// <summary>
        /// 单元
        /// </summary>
        [System.ComponentModel.Description("单元")]
        InnerModel = 1,
        /// <summary>
        /// 页码
        /// </summary>
        [System.ComponentModel.Description("页码")]
        ExternalModel = 2
    }
}
