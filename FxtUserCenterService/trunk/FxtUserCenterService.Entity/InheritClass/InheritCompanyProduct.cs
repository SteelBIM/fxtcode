using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity.InheritClass
{
    public class InheritCompanyProduct : CompanyProduct
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        /// <summary>
        /// 账号后缀
        /// </summary>
        [SQLReadOnly]
        public string companycode { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        [SQLReadOnly]
        public string smssendname { get; set; }
    }
}
