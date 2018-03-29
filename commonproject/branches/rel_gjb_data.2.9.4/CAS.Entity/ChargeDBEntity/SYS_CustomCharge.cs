using System.Collections.Generic;

namespace CAS.Entity.DBEntity
{
    public class SYS_ChargeCustom : SYSChargeCustom
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
        /// 适用业务阶段
        /// </summary>
        public string businessstagetypename
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
        /// <summary>
        /// 公司收费详细列表
        /// </summary>
        public List<SYSChargeCustomDetail> ChargeCustomDetailList
        {
            get;
            set;
        }
    }
}
