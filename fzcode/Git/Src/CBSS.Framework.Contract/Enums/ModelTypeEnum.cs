using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 模型分类
    /// </summary>
    public enum ModelTypeEnum
    {

        /// <summary>
        /// 课内模型
        /// </summary>
        [System.ComponentModel.Description("课内模型")]
        InnerModel = 1,
        /// <summary>
        /// 课外模型
        /// </summary>
        [System.ComponentModel.Description("课外模型")]
        ExternalModel = 2
    }
}
