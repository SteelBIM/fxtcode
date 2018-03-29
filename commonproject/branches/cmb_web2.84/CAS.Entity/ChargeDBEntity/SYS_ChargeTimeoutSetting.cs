using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class SYS_ChargeTimeoutSetting : SYSChargeTimeoutSetting
    {

        /// <summary>
        /// 委托（报告）类型名称
        /// </summary>
        public string entrusttypename
        {
            get;
            set;
        }
        /// <summary>
        /// 适用机构
        /// </summary>
        public string departmentsname
        {
            get;
            set;
        }

        /// <summary>
        ///适用业务类型
        /// </summary>
        public string businessname
        {
            get;
            set;
        }
        /// <summary>
        /// 适用客户单位
        /// </summary>
        public string customersname
        {
            get;
            set;
        }
    }
}
