using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 资源访问方式  1 新MOD  2 本地 3 第三方
    /// </summary>
    public enum SourceAccessModeEnum
    {

        /// <summary>
        /// MOD
        /// </summary>
        [System.ComponentModel.Description("新MOD资源")]
        InnerModel = 1,
        /// <summary>
        /// 上传
        /// </summary>
        [System.ComponentModel.Description("本地资源")]
        ExternalModel = 2,
        /// <summary>
        /// 第三方
        /// </summary>
        [System.ComponentModel.Description("第三方资源")]
        ThirdParty = 3
    }
}
