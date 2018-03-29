using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_ChargeEntrust : BaseTO
    {
        private long _eid;
        /// <summary>
        /// 委托id
        /// </summary>
        [SQLField("eid", EnumDBFieldUsage.PrimaryKey, true)]
        public long eid
        {
            get { return _eid; }
            set { _eid = value; }
        }

        private long _entrustid;
        /// <summary>
        /// 流水号,业务编号(自动生成的,其他表关联)2013030001
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        /// <summary>
        /// 报告id
        /// </summary>
        public long reportid
        {
            get;
            set;
        }
        private string _projectname;
        /// <summary>
        /// 项目名称
        /// </summary>


        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        public string clientname
        {
            get;
            set;
        }
        private string _customercompanyfullname;
        /// <summary>
        /// 客户机构全称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }

        private int? _businessuserid;
        /// <summary>
        /// 业务员
        /// </summary>
        public int? businessuserid
        {
            get { return _businessuserid; }
            set { _businessuserid = value; }
        }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public string businessusername
        {
            get;
            set;
        }
        private string _reportno;
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }
        /// <summary>
        /// 报告类型名称
        /// </summary>
        //public string reporttypename
        //{
        //    get;
        //    set;
        //}
        private decimal? _querytotalprice;
        /// <summary>
        /// 评估对象总价
        /// </summary>
        public decimal? querytotalprice
        {
            get { return _querytotalprice; }
            set { _querytotalprice = value; }
        }

        private decimal? _companystandardcharge;
        /// <summary>
        /// 公司标准收费
        /// </summary>
        public decimal? companystandardcharge
        {
            get { return _companystandardcharge; }
            set { _companystandardcharge = value; }
        }

        private decimal? _companymincharge;
        /// <summary>
        /// 公司最低收费
        /// </summary>
        public decimal? companymincharge
        {
            get { return _companymincharge; }
            set { _companymincharge = value; }
        }
        private decimal? _privilegediscount;
        /// <summary>
        /// 优惠折扣
        /// </summary>
        public decimal? privilegediscount
        {
            get { return _privilegediscount; }
            set { _privilegediscount = value; }
        }
        private decimal? _privilegemoney;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? privilegemoney
        {
            get { return _privilegemoney; }
            set { _privilegemoney = value; }
        }
        private decimal? _receivable;
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? receivable
        {
            get { return _receivable; }
            set { _receivable = value; }
        }

        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal? realityreceive
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get;
            set;
        }

        /// <summary>
        /// 结单时间
        /// </summary>
        public DateTime? overtime
        {
            get;
            set;
        }
        /// <summary>
        /// 收费状态码
        /// </summary>
        public int statusvalue
        {
            get;
            set;
        }
        /// <summary>
        /// 状态名
        /// </summary>
        public string statusName
        {
            get;
            set;
        }

        /// <summary>
        ///业务状态码
        /// </summary>
        public int statecode { get; set; }
        /// <summary>
        ///报告状态码
        /// </summary>
        public int reportstate { get; set; }

        public string reportstatename { get; set; }
        /// <summary>
        /// 业务类型,对公/个人
        /// </summary>
        public int businesstype
        {
            get;
            set;
        }

        public string businesstypename
        {
            get;
            set;

        }
        public int reporttypecode
        {
            get;
            set;
        }

        public string reporttypename
        {
            get;
            set;
        }
        /// <summary>
        /// 优惠类型
        /// </summary>
        public int privilegetype { get; set; }
        /// <summary>
        /// 最低收费
        /// </summary>
        public decimal? mincharge { get; set; }

        /// <summary>
        /// 审批类型id
        /// </summary>
        public int approvalid { get; set; }

        /// <summary>
        /// 报告完成时间
        /// </summary>
        public DateTime? completedate { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? closeaccounttime { get; set; }
        /// <summary>
        /// 退费时间
        /// </summary>
        public DateTime? refundtime { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal? refundamount { get; set; }
        /// <summary>
        /// 收费标准id
        /// </summary>
        public int? chargenameid { get; set; }

        public int worktodoid { get; set; }

        public string subcompanyname { get; set; }

        public int chargetimeout { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal billtotalamount { get; set; }

        /// <summary>
        /// 业务类型 预评、报告
        /// </summary>
        [SQLReadOnly]
        public int reportstage { get; set; }

        /// <summary>
        /// 收款负责人
        /// </summary>
        [SQLReadOnly]
        public string chargeusername { get; set; }
        /// <summary>
        /// 收费ID
        /// </summary>
        [SQLReadOnly]
        public int? chargeentrustid { get; set; }
        /// <summary>
        /// 预评ID
        /// </summary>
        [SQLReadOnly]
        public long ypid { get; set; }
        /// <summary>
        /// 是否月结
        /// </summary>
        [SQLReadOnly]
        public bool ischargemonthly { get; set; }

        /// <summary>
        /// 最大超时天数
        /// </summary>
        [SQLReadOnly]
        public int maxchargtimeoutday { get; set; }
        /// <summary>
        /// 返利
        /// </summary>
        [SQLReadOnly]
        public decimal? fanli { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        [SQLReadOnly]
        public decimal? costofproduction { get; set; }
        /// <summary>
        /// 最新一条收费日期
        /// </summary>
        [SQLReadOnly]
        public DateTime? lastinrecorddate { get; set; }
        /// <summary>
        /// 最新一条开票客户名称
        /// </summary>
        [SQLReadOnly]
        public string lastbillcustomername { get; set; }
        /// <summary>
        /// 最新一条开票日期
        /// </summary>
        [SQLReadOnly]
        public DateTime? lastbilltime { get; set; }
        /// <summary>
        /// 最新一条开票备注
        /// </summary>
        [SQLReadOnly]
        public string lastbillremark { get; set; }
        /// <summary>
        /// 0:没在审批中，1：在审批中
        /// </summary>
        [SQLReadOnly]
        public int isapprovaliding { get; set; }
        /// <summary>
        /// 坏账ID
        /// </summary>
        [SQLReadOnly]
        public int chargebadid { get; set; }
        /// <summary>
        /// 收费金额
        /// </summary>
        [SQLReadOnly]
        public decimal chargemoney { get; set; }
        /// <summary>
        /// 收费人
        /// </summary>
        [SQLReadOnly]
        public string chargeuser { get; set; }
        /// <summary>
        /// 收费时间
        /// </summary>
        [SQLReadOnly]
        public DateTime chargetime { get; set; }
        /// <summary>
        /// 支出类型
        /// </summary>
        [SQLReadOnly]
        public string outtypename { get; set; }
        /// <summary>
        /// 获取统计信息
        /// </summary>
        [SQLReadOnly]
        public ChargeInOutRecordSum chargeinoutrecordsum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为免单
        /// </summary>
        [SQLReadOnly]
        public int isfree
        {
            get;
            set;
        }
        /// <summary>
        /// 开票次数数
        /// </summary>
        [SQLReadOnly]
        public int billtotalnumber
        {
            get;
            set;
        }
        /// <summary>
        /// 收支中的收入次数
        /// </summary>
        [SQLReadOnly]
        public int receivetotalnumber
        {
            get;
            set;
        }

        /// <summary>
        /// 开票情况（可选列）
        /// 0：未开票，1：正常开票,2：已作废，3：红冲
        /// </summary>
        [SQLReadOnly]
        public int billdetailstatus
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [SQLReadOnly]
        public string remark
        {
            get;
            set;
        }
        /// <summary>
        /// 收款账户银行
        /// </summary>
        [SQLReadOnly]
        public string accountbank
        {
            get;
            set;
        }
        /// <summary>
        /// 收款账号
        /// </summary>
        [SQLReadOnly]
        public string accountname
        {
            get;
            set;
        }
        /// <summary>
        /// 开户名
        /// </summary>
        [SQLReadOnly]
        public string accounttruename
        {
            get;
            set;
        }
        /// <summary>
        /// (报告/预评)撰写人ID
        /// </summary>
        [SQLReadOnly]
        public int writerid
        {
            get;
            set;
        }
        /// <summary>
        /// (报告/预评)撰写人姓名
        /// </summary>
        [SQLReadOnly]
        public string writertruename
        {
            get;
            set;
        }

        /// <summary>
        /// (报告/预评)撰写人ID
        /// </summary>
        [SQLReadOnly]
        public int writerid1
        {
            get;
            set;
        }
        /// <summary>
        /// (报告/预评)撰写人姓名
        /// </summary>
        [SQLReadOnly]
        public string writername
        {
            get;
            set;
        }
    }
}
