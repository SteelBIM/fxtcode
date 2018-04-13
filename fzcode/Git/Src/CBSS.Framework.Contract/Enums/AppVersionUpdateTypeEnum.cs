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
    public enum AppVersionUpdateTypeEnum
    {

        /// <summary>
        /// 整包更新
        /// </summary>
        [System.ComponentModel.Description("整包更新")]
        AllPackagePpdate = 1,
        /// <summary>
        /// 增量更新
        /// </summary>
        [System.ComponentModel.Description("增量更新")]
        IncrementalUpdate = 2
    }
}
