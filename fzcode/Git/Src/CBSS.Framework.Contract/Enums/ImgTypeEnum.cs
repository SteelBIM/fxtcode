using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 图片格式，1：静态图 2：动态图 3：序列图
    /// </summary>
    public enum ImgTypeEnum
    {

        /// <summary>
        /// 静态图
        /// </summary>
        [System.ComponentModel.Description("静态图")]
        StaticImg= 1,
        /// <summary>
        /// 动态图
        /// </summary>
        [System.ComponentModel.Description("动态图")]
        DynamicImg= 2,
        /// <summary>
        /// 序列图
        /// </summary>
        [System.ComponentModel.Description("序列图")]
        SequenceImg= 3
    }
}
