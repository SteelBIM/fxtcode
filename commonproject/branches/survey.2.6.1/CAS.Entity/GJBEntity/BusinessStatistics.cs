using CAS.Entity.BaseDAModels;
using System.Collections.Generic;
using System;

namespace CAS.Entity.GJBEntity
{
    public class BusinessStatistics : BaseTO
    {
        public int id {get;set; }
        /// <summary>
        /// 统计名称
        /// </summary>
        public string name { get; set; }

        public string username { get; set; }
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
        /// 应收
        /// </summary>
        public decimal receivable { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal receipts { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal paymoney { get; set; }
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
        /// <summary>
        /// 业务编号
        /// </summary>
        public long entrustid { get; set; }
        /// <summary>
        /// 对公业务收入
        /// </summary>
        public decimal duigong { get; set; }
        /// <summary>
        /// 个人业务收入
        /// </summary>
        public decimal geren { get; set; }
        /// <summary>
        /// 待收金额
        /// </summary>
        public decimal daishoumoney { get; set; }
        /// <summary>
        /// 预评审批量
        /// </summary>
        public int ypapprovalcount { get; set; }
        /// <summary>
        /// 报告审批量
        /// </summary>
        public int reportapprovalcount { get; set; }
        /// <summary>
        /// 报告签字量
        /// </summary>
        public int reportsigncount { get; set; }
        /// <summary>
        /// 业务分配量
        /// </summary>
        public int businessassigncount { get; set; }
        /// <summary>
        /// 业务转交量
        /// </summary>
        public int businesszjcount { get; set; }
        /// <summary>
        /// 报告参与量
        /// </summary>
        public int reportcycount { get; set; }
        /// <summary>
        /// 询价审批量
        /// </summary>
        public int queryapprovalcount { get; set; }

        /// <summary>
        /// 报告错误量
        /// </summary>
        public int reporterrorcount { get; set; }
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

    public class RevenueLineStatistics
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 数据量集合
        /// </summary>
        public List<decimal> data { get; set; }
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
