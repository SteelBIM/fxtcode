using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity
{
    /// <summary>
    /// 外部api返回json实体
    /// </summary>
    public class OutJsonEntity
    {
        /// <summary>
        /// 返回ID
        /// 云查勘3.0 向第三方发起查勘，返回第三方查勘id
        /// </summary>
        public long objectid { get; set; }
    }
}
