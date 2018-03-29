using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class DATCAS_QueryHistory : DATCASQueryHistory
    {

        /// <summary>
        /// 多套ID
        /// </summary>
        [SQLReadOnly]
        public long mqid { get; set; }
        /// <summary>
        /// 房屋用途
        /// </summary>
        [SQLReadOnly]
        public string purpose { get; set; }
        /// <summary>
        /// 房屋用途名称
        /// </summary>
        [SQLReadOnly]
        public string purposename { get; set; }
        /// <summary>
        /// 建筑类型
        /// </summary>
        [SQLReadOnly]
        public string buildingtype { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [SQLReadOnly]
        public string housetype { get; set; }
        /// <summary>
        /// 户型名称
        /// </summary>
        [SQLReadOnly]
        public string housetypename { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [SQLReadOnly]
        public string department { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string businessuser { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [SQLReadOnly]
        public string customername { get; set; }
        /// <summary>
        /// 客户单位
        /// </summary>
        [SQLReadOnly]
        public string branchname { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [SQLReadOnly]
        public int departmentid { get; set; }

        /// <summary>
        /// 人工询价面积
        /// </summary>
        [SQLReadOnly]
        public string querybuildingarea { get; set; }
        /// <summary>
        /// 人工询价税费
        /// </summary>
        [SQLReadOnly]
        public decimal? querytax { get; set; }
        /// <summary>
        /// 人工询价净值
        /// </summary>
        [SQLReadOnly]
        public decimal? querynetprice { get; set; }
        /// <summary>
        /// 人工询价单价
        /// </summary>
        [SQLReadOnly]
        public decimal? queryunitprice { get; set; }
        /// <summary>
        /// 人工询价总价
        /// </summary>
        [SQLReadOnly]
        public decimal? querytotalprice { get; set; }
        /// <summary>
        /// 询价类型
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }
        /// <summary>
        /// 询价类型code
        /// </summary>
        [SQLReadOnly]
        public int? querytypecode { get; set; }
        /// <summary>
        /// 询价状态code
        /// </summary>
        [SQLReadOnly]
        public int? statecode { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string salemanname { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }
        /// <summary>
        /// 回价备注
        /// </summary>
        [SQLReadOnly]
        public string priceremark { get; set; }
        /// <summary>
        /// 人工询价回价时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? biddate { get; set; }
        /// <summary>
        /// 人工询价创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? querycreatedate { get; set; }
        /// <summary>
        /// 回价审批状态
        /// </summary>
        [SQLReadOnly]
        public int? approvalstatus { get; set; }

        /// <summary>
        /// 业务主键ID
        /// </summary>
        [SQLReadOnly]
        public long eid { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        [SQLReadOnly]
        public long entrustid { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        [SQLReadOnly]
        public int estatecode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        [SQLReadOnly]
        public string estatecodename { get; set; }
        /// <summary>
        /// 业务创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ecreatedate { get; set; }
        /// <summary>
        /// 预评ID
        /// </summary>
        [SQLReadOnly]
        public long ypid { get; set; }
        /// <summary>
        /// 预评状态
        /// </summary>
        [SQLReadOnly]
        public int ypstatecode { get; set; }
        /// <summary>
        /// 预评状态名称
        /// </summary>
        [SQLReadOnly]
        public string ypstatecodename { get; set; }
        /// <summary>
        /// 预评完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ypdate { get; set; }
        /// <summary>
        /// 预评创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? dycreatedate { get; set; }
        /// <summary>
        /// 报告ID
        /// </summary>
        [SQLReadOnly]
        public long reportid { get; set; }
        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public int reportstate { get; set; }
        /// <summary>
        /// 报告状态名称
        /// </summary>
        [SQLReadOnly]
        public string reportstatename { get; set; }
        /// <summary>
        /// 报告完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcompletedate { get; set; }
        /// <summary>
        /// 报告创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcreatedate { get; set; }
        /// <summary>
        /// 预评第三方信息ID
        /// </summary>
        [SQLReadOnly]
        public long dyetid
        {
            get;set;
        }
        /// <summary>
        /// 预评第三方信息是否已完成
        /// </summary>
        [SQLReadOnly]
        public bool? dyiscomplate
        {
            get;set;
        }
        /// <summary>
        /// 预评第三方信息状态
        /// </summary>
        [SQLReadOnly]
        public string dyexternalstatetext
        {
            get;set;
        }
        /// <summary>
        /// 预评第三方信息最后修改时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? dychangedon
        {
            get;
            set;
        }
        /// <summary>
        /// 自动询价盖章状态--潘锦发 2015-05-26
        /// </summary>
        [SQLReadOnly]
        public int? auotstampstatecode
        {
            get;
            set;
        }
        /// <summary>
        /// 报告第三方信息ID
        /// </summary>
        [SQLReadOnly]
        public long dretid
        {
            get;set;
        }
        /// <summary>
        /// 报告第三方信息是否已完成
        /// </summary>
        [SQLReadOnly]
        public bool? driscomplate
        {
            get;set;
        }
        /// <summary>
        /// 报告第三方信息状态
        /// </summary>
        [SQLReadOnly]
        public string drexternalstatetext
        {
            get;set;
        }
        /// <summary>
        /// 报告第三方信息最后修改时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drchangedon
        {
            get;
            set;
        }
        /// <summary>
        /// 数据来源，1-从询价添加,2-从委估对象添加的价格信息
        /// </summary>
        [SQLReadOnly]
        public int dataorigintype
        {
            get;
            set;
        }
        /// <summary>
        /// 是否先出预评
        /// </summary>
        [SQLReadOnly]
        public bool entrustyp
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款成数
        /// </summary>
        [SQLReadOnly]
        public string loanpernumname { get; set; }

        /// <summary>
        /// 人工估价 -贷款成数
        /// </summary>
        [SQLReadOnly]
        public string qloanpernumname { get; set; }

        /// <summary>
        /// 人工估价 -可贷金额
        /// </summary>
        [SQLReadOnly]
        public decimal? qloanablenum { get; set; }


        /// <summary>
        ///附属房屋类型名称
        /// </summary>
        [SQLReadOnly]
        public string subhousetypename { get; set; }
        #region  机构信息
        /// <summary>
        /// 机构名 人工询价
        /// </summary>
        [SQLReadOnly]
        public string qcustomercompanyname
        {
            get;
            set;
        }
        /// <summary>
        /// 机构名 自动询价
        /// </summary>
        [SQLReadOnly]
        public string qhcustomercompanyname
        {
            get;
            set;
        }
        /// <summary>
        /// 分支机构名 人工询价
        /// </summary>
        [SQLReadOnly]
        public string qbankbranchname
        {
            get;
            set;
        }
        /// <summary>
        /// 分支机构名 自动询价
        /// </summary>
        [SQLReadOnly]
        public string qhbankbranchname
        {
            get;
            set;
        }
        /// <summary>
        /// 支行名 人工询价
        /// </summary>
        [SQLReadOnly]
        public string qbanksubbranchname
        {
            get;
            set;
        }
        /// <summary>
        /// 支行名 自动询价
        /// </summary>
        [SQLReadOnly]
        public string qhbanksubbranchname
        {
            get;
            set;
        } 
        #endregion
    }
}
