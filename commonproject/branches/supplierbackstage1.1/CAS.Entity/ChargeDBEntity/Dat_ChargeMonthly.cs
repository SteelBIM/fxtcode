using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.DBEntity
{
    public class Dat_ChargeMonthly : DatChargeMonthly
    {
        /// <summary>
        /// 已实收
        /// </summary>
        public decimal? hadreceipts
        { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal? expenditure
        { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        public decimal? costofproduction
        { get; set; }

        /// <summary>
        /// 总应退
        /// </summary>
        public decimal? refundamount
        { get; set; }


    }
}
