using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// B端、C端
    /// </summary>
    public enum  BCPointEnum
    {
        /// <summary>
        /// B端
        /// </summary>
        [System.ComponentModel.Description("B端")]
        Bpoint =1,
        /// <summary>
        /// C端
        /// </summary>
        [System.ComponentModel.Description("C端")]
        Cpoint =2
    }
}
