using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.DBEntity
{
    public class SYS_CustomCharge : SYSCustomCharge
    {

        /// <summary>
        /// 委托类型名称
        /// </summary>
        public string EntrustTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 适用机构
        /// </summary>
        public string DepartmentsName
        {
            get;
            set;
        }

        /// <summary>
        ///适用业务类型
        /// </summary>
        public string BusinessName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司收费详细列表
        /// </summary>
        public List<SYSCustomChargeDetail> CustomChargeDetailList
        {
            get;
            set;
        }
    }
}
