using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Report")]
    public class DatReport : BaseTO
    {
        private long _reportid;
        [SQLField("reportid", EnumDBFieldUsage.PrimaryKey, true)]
        public long reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        private int? _subcompanyid;
        public int? subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
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
        private decimal? _querytotalprice;
        /// <summary>
        /// 评估对象总价
        /// </summary>
        public decimal? querytotalprice
        {
            get { return _querytotalprice; }
            set { _querytotalprice = value; }
        }
        private string _querytotalarea;
        /// <summary>
        /// 评估对象总面积
        /// </summary>
        public string querytotalarea
        {
            get { return _querytotalarea; }
            set { _querytotalarea = value; }
        }
        private string _reportsubtype;
        /// <summary>
        /// 报告细分类型((1007001))
        /// </summary>
        public string reportsubtype
        {
            get { return _reportsubtype; }
            set { _reportsubtype = value; }
        }
        private string _valuationmethods = "";
        /// <summary>
        /// 估价方法(1011)
        /// </summary>
        public string valuationmethods
        {
            get { return _valuationmethods; }
            set { _valuationmethods = value; }
        }
        private string _appraisers;
        /// <summary>
        /// 估价师
        /// </summary>
        public string appraisers
        {
            get { return _appraisers; }
            set { _appraisers = value; }
        }
        private int? _appraisersuserid1;
        /// <summary>
        /// 审核估价师
        /// </summary>
        public int? appraisersuserid1
        {
            get { return _appraisersuserid1; }
            set { _appraisersuserid1 = value; }
        }
        private string _appraisersusernumber1;
        /// <summary>
        /// 审核估价师注册号,来自privi_usercert
        /// </summary>
        public string appraisersusernumber1
        {
            get { return _appraisersusernumber1; }
            set { _appraisersusernumber1 = value; }
        }
        private int? _appraisersuserid2;
        /// <summary>
        /// 项目负责估价师
        /// </summary>
        public int? appraisersuserid2
        {
            get { return _appraisersuserid2; }
            set { _appraisersuserid2 = value; }
        }
        private string _appraisersusernumber2;
        /// <summary>
        /// 项目负责估价师注册号,来自privi_usercert
        /// </summary>
        public string appraisersusernumber2
        {
            get { return _appraisersusernumber2; }
            set { _appraisersusernumber2 = value; }
        }
        private DateTime? _reportbegindate;
        /// <summary>
        /// 报告作业开始时间
        /// </summary>
        public DateTime? reportbegindate
        {
            get { return _reportbegindate; }
            set { _reportbegindate = value; }
        }
        private DateTime? _reportenddate;
        /// <summary>
        /// 报告作业结束时间
        /// </summary>
        public DateTime? reportenddate
        {
            get { return _reportenddate; }
            set { _reportenddate = value; }
        }
        private DateTime? _reportvalidbegindate;
        /// <summary>
        /// 报告有效开始时间
        /// </summary>
        public DateTime? reportvalidbegindate
        {
            get { return _reportvalidbegindate; }
            set { _reportvalidbegindate = value; }
        }
        private DateTime? _reportvalidenddate;
        /// <summary>
        /// 报告有效结束时间
        /// </summary>
        public DateTime? reportvalidenddate
        {
            get { return _reportvalidenddate; }
            set { _reportvalidenddate = value; }
        }
        private DateTime? _valuationdate;
        /// <summary>
        /// 估价时间
        /// </summary>
        public DateTime? valuationdate
        {
            get { return _valuationdate; }
            set { _valuationdate = value; }
        }
        private int? _writerid;
        /// <summary>
        /// 报告撰写人ID
        /// </summary>
        public int? writerid
        {
            get { return _writerid; }
            set { _writerid = value; }
        }
        private int? _reportstate;
        /// <summary>
        /// 报告状态10009
        /// </summary>
        public int? reportstate
        {
            get { return _reportstate; }
            set { _reportstate = value; }
        }
        private int _createuser;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _completedate;
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? completedate
        {
            get { return _completedate; }
            set { _completedate = value; }
        }
        private int? _printnumber;
        /// <summary>
        /// 打印份数
        /// </summary>
        public int? printnumber
        {
            get { return _printnumber; }
            set { _printnumber = value; }
        }
        private DateTime? _printdate;
        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime? printdate
        {
            get { return _printdate; }
            set { _printdate = value; }
        }
        private int? _printuser;
        /// <summary>
        /// 打印人
        /// </summary>
        public int? printuser
        {
            get { return _printuser; }
            set { _printuser = value; }
        }
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号,从委托表来
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private int _approvalstatus = 1;
        /// <summary>
        /// 审批状态 1未参与审批 2正在审批中 3审批完成
        /// </summary>
        public int approvalstatus
        {
            get { return _approvalstatus; }
            set { _approvalstatus = value; }
        }
        private int? _worktodoid;
        public int? worktodoid
        {
            get { return _worktodoid; }
            set { _worktodoid = value; }
        }
        private bool _performancedependent = false;
        /// <summary>
        /// 是否参与业绩统计 1为参与
        /// </summary>
        public bool performancedependent
        {
            get { return _performancedependent; }
            set { _performancedependent = value; }
        }

        private bool _issure = false;
        /// <summary>
        /// 是否需评跟进 1为跟进
        /// </summary>
        public bool issure
        {
            get { return _issure; }
            set { _issure = value; }
        }
        private bool _stamped = false;
        /// <summary>
        /// 已盖章 1为盖章
        /// </summary>
        public bool stamped
        {
            get { return _stamped; }
            set { _stamped = value; }
        }
        private int _reportedresults = 0;
        /// <summary>
        /// 上报协会 是否
        /// </summary>
        public int reportedresults
        {
            get { return _reportedresults; }
            set { _reportedresults = value; }
        }
        private bool _reportinvalid = false;
        /// <summary>
        /// 报告作废 1为作废
        /// </summary>
        public bool reportinvalid
        {
            get { return _reportinvalid; }
            set { _reportinvalid = value; }
        }
        private bool _ispricedispute = false;
        /// <summary>
        /// 是否价格争议 1是
        /// </summary>
        public bool ispricedispute
        {
            get { return _ispricedispute; }
            set { _ispricedispute = value; }
        }
        private int _pricedisputeamount = 0;
        /// <summary>
        /// 价格争议额度
        /// </summary>
        public int pricedisputeamount
        {
            get { return _pricedisputeamount; }
            set { _pricedisputeamount = value; }
        }
        private decimal? _priceonrequest;
        /// <summary>
        /// 存在价格争议时，客户经理要求的价格
        /// </summary>
        public decimal? priceonrequest
        {
            get { return _priceonrequest; }
            set { _priceonrequest = value; }
        }
        private string _pricedisputeremark;
        /// <summary>
        /// 价格争议备注
        /// </summary>
        public string pricedisputeremark
        {
            get { return _pricedisputeremark; }
            set { _pricedisputeremark = value; }
        }
        private decimal? _costofproduction;
        /// <summary>
        /// 报告作废工本费
        /// </summary>
        public decimal? costofproduction
        {
            get { return _costofproduction; }
            set { _costofproduction = value; }
        }
        private string _customername;
        /// <summary>
        /// 报告使用方
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        /// <summary>
        /// 是否有效，用作删除标记
        /// </summary>
        public bool? valid { get; set; }

        /// <summary>
        /// 税费
        /// </summary>
        public decimal? tax { get; set; }
        /// <summary>
        /// 净值
        /// </summary>
        public decimal? netprice { get; set; }
        private bool? _outcancel;
        /// <summary>
        /// 外部 是否撤销预评
        /// </summary>
        public bool? outcancel
        {
            get { return _outcancel; }
            set { _outcancel = value; }
        }
        private string _linkman;
        /// <summary>
        /// 外部 联系人
        /// </summary>
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private string _linkmanphone;
        /// <summary>
        /// 外部 联系电话
        /// </summary>
        public string linkmanphone
        {
            get { return _linkmanphone; }
            set { _linkmanphone = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _outremark;
        /// <summary>
        /// 机构备注
        /// </summary>
        public string outremark
        {
            get { return _outremark; }
            set { _outremark = value; }
        }
        private decimal? _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private string _revokeremark;
        /// <summary>
        /// 撤销备注
        /// </summary>
        public string revokeremark
        {
            get { return _revokeremark; }
            set { _revokeremark = value; }
        }
        private int? _assignoutpersonid;
        /// <summary>
        /// 转出人
        /// </summary>
        public int? assignoutpersonid
        {
            get { return _assignoutpersonid; }
            set { _assignoutpersonid = value; }
        }
        private int? _assigninpersonid;
        /// <summary>
        /// 转入人
        /// </summary>
        public int? assigninpersonid
        {
            get { return _assigninpersonid; }
            set { _assigninpersonid = value; }
        }
        private int? _assignstate;
        /// <summary>
        /// 转交状态
        /// </summary>
        public int? assignstate
        {
            get { return _assignstate; }
            set { _assignstate = value; }
        }
        private DateTime? _assigndate;
        /// <summary>
        /// 转交时间
        /// </summary>
        public DateTime? assigndate
        {
            get { return _assigndate; }
            set { _assigndate = value; }
        }
        private bool _isgetno = false;
        /// <summary>
        /// 是否领取编号
        /// </summary>
        public bool isgetno
        {
            get { return _isgetno; }
            set { _isgetno = value; }
        }
        private bool? _changedreport;
        /// <summary>
        /// 修改报告。为1则表示报告是从已完成状态更改为撰写中
        /// </summary>
        public bool? changedreport
        {
            get { return _changedreport; }
            set { _changedreport = value; }
        }
        private decimal? _totalhouseprice;
        /// <summary>
        /// 主房总价
        /// </summary>
        public decimal? totalhouseprice
        {
            get { return _totalhouseprice; }
            set { _totalhouseprice = value; }
        }
        private decimal? _totalsubhouseprice;
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        public decimal? totalsubhouseprice
        {
            get { return _totalsubhouseprice; }
            set { _totalsubhouseprice = value; }
        }
        private decimal? _totallandprice;
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? totallandprice
        {
            get { return _totallandprice; }
            set { _totallandprice = value; }
        }
        private decimal? _submittalsmarketprice;
        /// <summary>
        /// 送审市场价
        /// </summary>
        public decimal? submittalsmarketprice
        {
            get { return _submittalsmarketprice; }
            set { _submittalsmarketprice = value; }
        }
        private decimal? _marketpricedeviation;
        /// <summary>
        /// 送审偏差率
        /// </summary>
        public decimal? marketpricedeviation
        {
            get { return _marketpricedeviation; }
            set { _marketpricedeviation = value; }
        }

        private int _reportnotype;
        /// <summary>
        /// 报告编号类型
        /// </summary>
        public int reportnotype
        {
            get { return _reportnotype; }
            set { _reportnotype = value; }
        }

        private string _totallandarea;
        /// <summary>
        /// 土地总面积
        /// </summary>
        public string totallandarea
        {
            get { return _totallandarea; }
            set { _totallandarea = value; }
        }

        private int? _backupdatauser;
        /// <summary>
        /// 归档资料负责人
        /// </summary>
        public int? backupdatauser
        {
            get { return _backupdatauser; }
            set { _backupdatauser = value; }
        }
        private decimal? _legalpayment;
        /// <summary>
        /// 法定优先受偿款总额
        /// </summary>
        public decimal? legalpayment
        {
            get { return _legalpayment; }
            set { _legalpayment = value; }
        }
        private int? _tldepartmentid;
        /// <summary>
        /// 技术部门
        /// </summary>
        public int? tldepartmentid
        {
            get { return _tldepartmentid; }
            set { _tldepartmentid = value; }
        }
        private decimal? _shouldfilllandprice;
        /// <summary>
        /// 应补地价总额
        /// </summary>
        public decimal? shouldfilllandprice
        {
            get { return _shouldfilllandprice; }
            set { _shouldfilllandprice = value; }
        }
        private string _assistedwriterids;
        /// <summary>
        /// 报告撰写辅助人
        /// </summary>
        public string assistedwriterids
        {
            get { return _assistedwriterids; }
            set { _assistedwriterids = value; }
        }
        private int? _assignuserid;
        /// <summary>
        /// 分配人ID
        /// </summary>
        public int? assignuserid
        {
            get { return _assignuserid; }
            set { _assignuserid = value; }
        }
        private DateTime? _assigndatetime;
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? assigndatetime
        {
            get { return _assigndatetime; }
            set { _assigndatetime = value; }
        }

        private decimal? _liquidityvaluesum;
        /// <summary>
        /// 强制变现值总额
        /// </summary>
        public decimal? liquidityvaluesum
        {
            get { return _liquidityvaluesum; }
            set { _liquidityvaluesum = value; }
        }

        private decimal? _liquiditytaxsumvalue;
        /// <summary>
        /// 强制变现税费总额
        /// </summary>
        public decimal? liquiditytaxsumvalue
        {
            get { return _liquiditytaxsumvalue; }
            set { _liquiditytaxsumvalue = value; }
        }
        /// <summary>
        /// 撰写联系方式
        /// </summary>
        public string writermobile { get; set; }
        
        /// <summary>
        /// 是否以复评
        /// </summary>
        public int isfp { get; set; }

        private string _changedreportreason;
        /// <summary>
        /// 报告修改理由
        /// </summary>
        public string changedreportreason
        {
            get { return _changedreportreason; }
            set { _changedreportreason = value; }
        }
        private int? _reportprintuser;
        /// <summary>
        /// 报告打印人
        /// </summary>
        public int? reportprintuser
        {
            get { return _reportprintuser; }
            set { _reportprintuser = value; }
        }
        private DateTime? _reportprinttime;
        /// <summary>
        /// 报告打印时间
        /// </summary>
        public DateTime? reportprinttime
        {
            get { return _reportprinttime; }
            set { _reportprinttime = value; }
        }
        private int? _reportprintnumber;
        /// <summary>
        /// 报告打印总份数
        /// </summary>
        public int? reportprintnumber
        {
            get { return _reportprintnumber; }
            set { _reportprintnumber = value; }
        }
        private bool? _reportprintstatus;
        /// <summary>
        /// 报告打印状态
        /// </summary>
        public bool? reportprintstatus
        {
            get { return _reportprintstatus; }
            set { _reportprintstatus = value; }
        }
        private int? _reportsealuser;
        /// <summary>
        /// 报告盖章人
        /// </summary>
        public int? reportsealuser
        {
            get { return _reportsealuser; }
            set { _reportsealuser = value; }
        }
        private DateTime? _reportsealtime;
        /// <summary>
        /// 报告盖章时间
        /// </summary>
        public DateTime? reportsealtime
        {
            get { return _reportsealtime; }
            set { _reportsealtime = value; }
        }
        private bool? _reportsealstatus;
        /// <summary>
        /// 报告盖章状态
        /// </summary>
        public bool? reportsealstatus
        {
            get { return _reportsealstatus; }
            set { _reportsealstatus = value; }
        }
        private string _reportsealremark;
        /// <summary>
        /// 报告盖章备注
        /// </summary>
        public string reportsealremark
        {
            get { return _reportsealremark; }
            set { _reportsealremark = value; }
        }
        private string _invalidremark;
        /// <summary>
        /// 报告作废备注
        /// </summary>
        public string invalidremark
        {
            get { return _invalidremark; }
            set { _invalidremark = value; }
        }

        private DateTime? _approvaldate;
        /// <summary>
        /// 报告审批时间
        /// </summary>
        public DateTime? approvaldate
        {
            get { return _approvaldate; }
            set { _approvaldate = value; }
        }

        private decimal? _decoratealltotalprice;
        /// <summary>
        /// 装修总值
        /// </summary>
        public decimal? decoratealltotalprice
        {
            get { return _decoratealltotalprice; }
            set { _decoratealltotalprice = value; }
        }
        private decimal? _movablealltotalprice;
        /// <summary>
        /// 动产总值
        /// </summary>
        public decimal? movablealltotalprice
        {
            get { return _movablealltotalprice; }
            set { _movablealltotalprice = value; }
        }
        private decimal? _othervalue;
        /// <summary>
        /// 其他价值总值
        /// </summary>
        public decimal? othervalue
        {
            get { return _othervalue; }
            set { _othervalue = value; }
        }
        //private bool _ischangedreport = false;
        ///// <summary>
        ///// 已完成的报告是否可以修改
        ///// </summary>
        //[SQLReadOnly]
        //public bool ischangedreport
        //{
        //    get { return _ischangedreport; }
        //    set { _ischangedreport = value; }
        //}
        /*
        private string _personalfactors;
        /// <summary>
        /// 个别因素分析
        /// </summary>
        public string personalfactors
        {
            get
            {
                return _personalfactors;
            }
            set
            {
                _personalfactors = value;
            }
        }
        private string _personalfactorsids;
        /// <summary>
        /// 个别因素分析ID
        /// </summary>
        public string personalfactorsids
        {
            get
            {
                return _personalfactorsids;
            }
            set
            {
                _personalfactorsids = value;
            }

        }     
        */
    }
}