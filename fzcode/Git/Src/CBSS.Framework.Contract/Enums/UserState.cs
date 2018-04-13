using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserState”的 XML 注释
    public enum UserState
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserState”的 XML 注释
    {

        /// <remarks/>
        notLogin,

        /// <remarks/>
        notOnLine,

        /// <remarks/>
        otherOnline,

        /// <remarks/>
        selfOnline,

        /// <remarks/>
        appForbidden,
    }
}
