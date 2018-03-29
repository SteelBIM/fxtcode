using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendUserInfo : UserInfo
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        /// <summary>
        /// 公司名称简称
        /// </summary>
        [SQLReadOnly]
        public string shortname { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        [SQLReadOnly]
        public int provinceid { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        [SQLReadOnly]
        public int cityid { get; set; }
    }
}
