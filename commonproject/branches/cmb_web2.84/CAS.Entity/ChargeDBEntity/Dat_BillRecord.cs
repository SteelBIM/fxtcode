using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_ChargeBillRecord : DatChargeBillRecord
    {
        /// <summary>
        /// 账单类型名称
        /// </summary>
        public string billtypename { get; set; }
        /// <summary>
        /// 账单抬头名称
        /// </summary>
        public string billtitlename { get; set; }
        /// <summary>
        /// 开票人真名
        /// </summary>
        public string drawertruename { get; set; }


        /// <summary>
        /// 缴费人/委托人
        /// </summary>
        public string clientname { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string businessusername { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string biztypename { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypecodename { get; set; }
        /// <summary>
        /// 分支机构
        /// </summary>
        public string subcompanyname { get; set; }
        /// <summary>
        /// 客户结构全称
        /// </summary>
        public string customercompanyfullname { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno { get; set; }
        /// <summary>
        /// 业务表ID
        /// </summary>
        public long? eid { get; set; }
    }
}
