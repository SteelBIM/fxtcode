using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendDatAmountDetails : DatAmountDetails
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        [SQLReadOnly]
        public string filename { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        [SQLReadOnly]
        public string filepath { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SQLReadOnly]
        public string month { get; set; }
        /// <summary>
        /// 当月有多少笔结算
        /// </summary>
        [SQLReadOnly]
        public int monthnumber { get; set; }
    }
}
