using CAS.Entity.BaseDAModels;
using System.Collections.Generic;
using System;

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
        /// <summary>
        /// 询价转预评量
        /// </summary>
        public string queryyp { get; set; }
        /// <summary>
        /// 询价转报告量
        /// </summary>
        public string queryreport { get; set; }
        /// <summary>
        /// 预评转报告量
        /// </summary>
        public string ypreport { get; set; }
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

    /// <summary>
    /// 业务转化率相关数据
    /// </summary>
    public class BusinessRateStatistics : BaseTO
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 预评业务ID
        /// </summary>
        public long? ypentrustid { get; set; }
        /// <summary>
        /// 报告业务ID
        /// </summary>
        public long? reportentrustid { get; set; }
        /// <summary>
        /// 是否出预评
        /// </summary>
        public bool entrustyp { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public int statecode { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int biztype { get; set; }
    }
}
