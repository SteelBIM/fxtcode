using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 报告退费统计
    /// </summary>
    public class Dat_ReportRefund : BaseTO
    {
        /// <summary>
        ///委托id
        /// </summary>
        [SQLField("billrecordid", EnumDBFieldUsage.PrimaryKey, true)]
        public long EntrustId
        { get; set; }
        /// <summary>
        /// 总应收
        /// </summary>
        public decimal? Receivable
        { get; set; }

        /// <summary>
        /// 已实收
        /// </summary>
        public decimal? Receipts
        { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal? Expenditure
        { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        public decimal? CostOfProduction
        { get; set; }

        /// <summary>
        /// 总应退
        /// </summary>
        public decimal? RefundAmount
        { get; set; }

        /// <summary>
        /// 收费状态
        /// </summary>
        public string ChargeState
        { get; set; }


    }
}
