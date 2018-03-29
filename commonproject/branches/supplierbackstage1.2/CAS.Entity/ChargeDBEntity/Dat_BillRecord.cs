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
    }
}
