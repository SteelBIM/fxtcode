using System;
using System.Collections.Generic;
using CAS.Entity.BaseDAModels;
using CAS.Entity.GJBEntity;

namespace CAS.Entity.ExternalApi
{
    public class ExternalReport : BaseTO
    {
        public long ReportId { get; set; }
        /// <summary>
        /// 业务编号 项目编号
        /// </summary>
        public long EntrustId { get; set; }
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        public int ReportTypeCode { get; set; }
        /// <summary>
        /// 报告类型中文
        /// </summary>
        public string ReportTypeName { get; set; }
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string ReportSubTypeName { get; set; }
        /// <summary>
        /// 权利人ID
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// 权利人
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectAddress { get; set; }
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string ClientContact { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNumber { get; set; }
        /// <summary>
        /// 委托方
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 物业类型code
        /// </summary>
        public int ObjectTypeCode { get; set; }
        /// <summary>
        /// 物业类型
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// 评估总价
        /// </summary>
        public Decimal? Totalprice { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string ClientType { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public int BusinessUserCode { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string BusinessUserName { get; set; }
        /// <summary>
        /// 报告撰写人编号
        /// </summary>
        public int ReportWriterCode { get; set; }
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public string ReportWriter { get; set; }
        /// <summary>
        /// 出报告日期
        /// </summary>
        public string CompleteDate { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public string Totalarea { get; set; }
        /// <summary>
        /// 用地面积
        /// </summary>
        public string LandArea { get; set; }
        /// <summary>
        /// 估价时点
        /// </summary>
        public string ValuationDate { get; set; }
        /// <summary>
        /// 房产证号
        /// </summary>
        public string HouseCertNo { get; set; }
        /// <summary>
        /// 土地证载用途
        /// </summary>
        public string LandStatutoryPurpose { get; set; }
        /// <summary>
        /// 土地性质编号
        /// </summary>
        public int LandUseTypeCode { get; set; }
        /// <summary>
        /// 土地性质
        /// </summary>
        public string LandUseType { get; set; }
        /// <summary>
        /// 成新率
        /// </summary>
        public string NewnessRate { get; set; }
        /// <summary>
        /// 估价单价
        /// </summary>
        public Decimal? UnitPrice { get; set; }
        /// <summary>
        /// 评估目的
        /// </summary>
        public string AssessPurpose { get; set; }
        /// <summary>
        /// 评估方法
        /// </summary>
        public string ValuationMethods { get; set; }
        /// <summary>
        /// 估价是编号
        /// </summary>
        public int AppraiserCode { get; set; }
        /// <summary>
        /// 估价师
        /// </summary>
        public string Appraiser { get; set; }
        /// <summary>
        /// 查勘员名称
        /// </summary>
        public string SurveyUserName { get; set; }
        /// <summary>
        /// 查勘员
        /// </summary>
        public string SurveyUser { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        public string ReportBeginDate { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        public string ReportEndDate { get; set; }
        /// <summary>
        /// 委托类型编号
        /// </summary>
        public int EntrustTypeCode { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        public string EntrustType { get; set; }
        /// <summary>
        /// 贷款银行编号
        /// </summary>
        public int LoanBankCode { get; set; }
        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LoanBank { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public string SubmitDate { get; set; }
        /// <summary>
        /// 委托人电话
        /// </summary>
        public string ClientPhone { get; set; }
        /// <summary>
        /// 委托人邮编
        /// </summary>
        public string ClientZip { get; set; }
        /// <summary>
        /// 委托人地址
        /// </summary>
        public string ClientAddress { get; set; }
        /// <summary>
        /// 土地发证日期
        /// </summary>
        public string LandUseCertDate { get; set; }
        /// <summary>
        /// 签字人
        /// </summary>
        public string SignaturePerson { get; set; }
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string BankUserName { get; set; }
        /// <summary>
        /// 业务联系人电话
        /// </summary>
        public string BankUserPhone { get; set; }
        /// <summary>
        /// 业务类型 code 2018
        /// </summary>
        public int biztype { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<Dat_Files> fileList { get; set; }
    }
}
