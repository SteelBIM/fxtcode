using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

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
        /// 业务跟进量
        /// </summary>
        public int followcount { get; set; }
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
        /// 提成金额
        /// </summary>
        public decimal brokerage { get; set; }

        /// <summary>
        /// 报告错误量
        /// </summary>
        public int reporterrorcount { get; set; }

        /// <summary>
        /// 询价纠错量 Alex 2016-08-08
        /// </summary>
        public int queryadjustcount { get; set; }

        /// <summary>
        /// 报告打印量
        /// </summary>
        public int reportprintcount { get; set; }
        /// <summary>
        /// 未收款  Alex 2016-12-29
        /// </summary>
        public decimal waitmoney { get; set; }
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
        /// <summary>
        /// 分类项集合
        /// </summary>
        public List<string> xlines { get; set; }
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
        /// <summary>
        /// 分类项集合
        /// </summary>
        public List<string> xlines { get; set; }
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

    /// <summary>
    /// 业务进度统计
    /// </summary>
    public class TaskStatistics : BaseTO
    {

        /// <summary>
        /// 待回价
        /// </summary>
        public int qDaiHuiJia { get; set; }
        /// <summary>
        /// 询价待分配
        /// </summary>
        public int qDaiFenPei { get; set; }
        /// <summary>
        /// 询价待审批
        /// </summary>
        public int qDaiShenPi { get; set; }
        /// <summary>
        /// 多套待回价
        /// </summary>
        public int mqDaiHuiJia { get; set; }
        /// <summary>
        /// 多套待分配
        /// </summary>
        public int mqDaiFenPei { get; set; }
        /// <summary>
        /// 多套待审批
        /// </summary>
        public int mqDaiShenPi { get; set; }
        /// <summary>
        /// 业务待分配
        /// </summary>
        public int entrustDaiFenPei { get; set; }
        /// <summary>
        /// 报告待撰写
        /// </summary>
        public int reportDaiZhuanXie { get; set; }
        /// <summary>
        /// 报告待分配
        /// </summary>
        public int reportDaiFenPei { get; set; }
        /// <summary>
        /// 报告待审批
        /// </summary>
        public int reportDaiShenpi { get; set; }
        /// <summary>
        /// 预评待撰写
        /// </summary>
        public int ypDaiZhuanXie { get; set; }
        /// <summary>
        /// 预评待审批
        /// </summary>
        public int ypDaiShenPi { get; set; }
        /// <summary>
        /// 行政待审批
        /// </summary>
        public int officeDaiShenPi { get; set; }
        /// <summary>
        /// 待投递
        /// </summary>
        public int deliverDaiTouDi { get; set; }
        /// <summary>
        /// 未收费
        /// </summary>
        public int chargeWeiShouFei { get; set; }
        /// <summary>
        /// 待退费
        /// </summary>
        public int chargeTuiFei { get; set; }
        /// <summary>
        /// 待结单
        /// </summary>
        public int chargeJieDan { get; set; }
        /// <summary>
        /// 未归档
        /// </summary>
        public int notBackUp { get; set; }
        /// <summary>
        /// 归档未完成
        /// </summary>
        public int notCompletedBackUp { get; set; }
        /// <summary>
        /// 报告作废
        /// </summary>
        public int reportZuoFei { get; set; }
        /// <summary>
        /// 业务撤销
        /// </summary>
        public int entrustCheXiao { get; set; }
        /// <summary>
        /// 预评未打印
        /// </summary>
        public int ypNotPrint { get; set; }
        /// <summary>
        /// 预评未盖章
        /// </summary>
        public int ypNotSeal { get; set; }
        /// <summary>
        /// 报告未打印
        /// </summary>
        public int reportNotPrint { get; set; }
        /// <summary>
        /// 报告未盖章
        /// </summary>
        public int reportNotSeal { get; set; }
        /// <summary>
        /// 待查勘
        /// </summary>
        public int notSurvey { get; set; }
        /// <summary>
        /// 查勘中
        /// </summary>
        public int surveying { get; set; }
        /// <summary>
        /// 收费审批中
        /// </summary>
        public int chargeDaiShenPi { get; set; }

        
    }


    public class SurveyStatistics
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 当前查勘量
        /// </summary>
        public int createcount { get; set; }
        /// <summary>
        /// 查勘总量
        /// </summary>
        public int totalcount { get; set; }
    }
}
