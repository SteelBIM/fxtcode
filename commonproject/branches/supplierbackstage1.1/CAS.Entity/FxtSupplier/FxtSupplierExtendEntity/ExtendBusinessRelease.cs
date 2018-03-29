using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendBusinessRelease : BusinessRelease
    {
        /// <summary>
        /// 供应商简称
        /// </summary>
        public string shortname { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public int businessid { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname { get; set; }
    }
}
