using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{

    public class ExtendDatSettle : DatSettle 
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SQLReadOnly]
        public string shortname { get; set; }
    }
}
