using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 模型资源分类
    /// </summary>
    public enum ModelResourceType
    {

        /// <summary>
        /// 原生
        /// </summary>
        [System.ComponentModel.Description("原生")]
        protogenesis = 1,
        /// <summary>
        /// H5
        /// </summary>
        [System.ComponentModel.Description("H5")]
        h5 = 2,
    }
}
