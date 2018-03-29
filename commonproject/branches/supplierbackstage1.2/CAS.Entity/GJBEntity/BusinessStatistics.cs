using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

namespace CAS.Entity.GJBEntity
{
    public class BusinessStatistics : BaseTO
    {
        public string name { get; set; }
        /// <summary>
        /// 询价量
        /// </summary>
        public int querycount { get; set; }
        /// <summary>
        /// 预评量
        /// </summary>
        public int ypcount { get; set; }
        /// <summary>
        /// 报告量
        /// </summary>
        public int reportcount { get; set; }
        /// <summary>
        /// 查勘量
        /// </summary>
        public int surveycount { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal receipts { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        public decimal receivable { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal brokerage { get;set; }
        /// <summary>
        /// 绩效
        /// </summary>
        public int appraisalstage { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public string statisticsdate { get; set; }
    }

    public class BusinessLineStatistics
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string name{ get; set; }
        /// <summary>
        /// 数据量集合
        /// </summary>
        public List<int> data { get; set; }
    }

    public class BusinessColumnStatistics
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 数据量集合
        /// </summary>
        public List<double> data { get; set; }
    }
}
