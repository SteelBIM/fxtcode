using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 商品出售方式1单册2套餐
    /// </summary>
    public enum GoodWayEnum
    {

        /// <summary>
        /// 单册
        /// </summary>
        [System.ComponentModel.Description("单册")] 
        SingleVolume = 1,
        /// <summary>
        /// 套餐
        /// </summary>
        [System.ComponentModel.Description("套餐")]
        SetMeal = 2
    }
}
