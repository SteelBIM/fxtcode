using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 报告退费统计
    /// </summary>
    public class Dat_ReportRefund : BaseTO
    {
        /// <summary>
        ///id
        /// </summary>     
        public int entrustrefundid
        { get; set; }
        
        /// <summary>
        ///委托id
        /// </summary>     
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
        ///已退
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
