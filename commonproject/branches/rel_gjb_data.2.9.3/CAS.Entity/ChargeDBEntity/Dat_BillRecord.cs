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
        /// <summary>
        /// 用于各项需要统计的开票金额
        /// </summary>
        public ChargeBillRecordSum chargebillrecordsum { get; set; }

        /// <summary>
        /// 预评编号  Alex Add 2015-09-16
        /// </summary>
        [SQLReadOnly]
        public string ypnumber { get; set; }
    }


    [Serializable]
    /// <summary>
    /// 用于各项需要统计的开票金额
    /// </summary>    
    public class ChargeBillRecordSum : BaseTO
    {
        /// <summary>
        /// 总开票金额
        /// </summary>
        public decimal zongjine { get; set; }

    }
}
