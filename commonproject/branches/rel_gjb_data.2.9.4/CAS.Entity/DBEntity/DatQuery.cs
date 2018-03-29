using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Query")]
    public class DatQuery : BaseTO
    {
        private long _qid;
        [SQLField("qid", EnumDBFieldUsage.PrimaryKey, true)]
        public long qid
        {
            get { return _qid; }
            set { _qid = value; }
        }
        private long _objectid = 0;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private string _projectfullname;
        public string projectfullname
        {
            get { return _projectfullname; }
            set { _projectfullname = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid = 0;
        /// <summary>
        /// 评估公司ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
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
        private int _querytypecode = 1031001;
        /// <summary>
        /// 询价类型(1031)
        /// </summary>
        public int querytypecode
        {
            get { return _querytypecode; }
            set { _querytypecode = value; }
        }
        private int _righttypecode = 0;
        /// <summary>
        /// 业务类型(对公/个人)200104
        /// </summary>
        public int righttypecode
        {
            get { return _righttypecode; }
            set { _righttypecode = value; }
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
        private decimal? _totalprice;
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private decimal? _tax;
        /// <summary>
        /// 税费
        /// </summary>
        public decimal? tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal? _netprice;
        /// <summary>
        /// 净值
        /// </summary>
        public decimal? netprice
        {
            get { return _netprice; }
            set { _netprice = value; }
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
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        
        private int? _statecode;
        /// <summary>
        /// 询价业务状态
        /// </summary>
        public int? statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        private int? _appraisersuserid;
        /// <summary>
        /// 估价师
        /// </summary>
        public int? appraisersuserid
        {
            get { return _appraisersuserid; }
            set { _appraisersuserid = value; }
        }
        private DateTime? _biddate;
        /// <summary>
        /// 回价时间
        /// </summary>
        public DateTime? biddate
        {
            get { return _biddate; }
            set { _biddate = value; }
        }
        private int? _salemanid;
        /// <summary>
        /// 业务员用户Id
        /// </summary>
        public int? salemanid
        {
            get { return _salemanid; }
            set { _salemanid = value; }
        }
        private int? _bankid;
        /// <summary>
        /// 委托银行总行
        /// </summary>
        public int? bankid
        {
            get { return _bankid; }
            set { _bankid = value; }
        }
        private int? _bankbranchid;
        /// <summary>
        /// 委托银行分行
        /// </summary>
        public int? bankbranchid
        {
            get { return _bankbranchid; }
            set { _bankbranchid = value; }
        }
        private int? _banksubbranchid;
        /// <summary>
        /// 委托银行支行
        /// </summary>
        public int? banksubbranchid
        {
            get { return _banksubbranchid; }
            set { _banksubbranchid = value; }
        }
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号，委托编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _contactuser;
        /// <summary>
        /// 联系人
        /// </summary>
        public string contactuser
        {
            get { return _contactuser; }
            set { _contactuser = value; }
        }
        private string _contacttelephone;
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string contacttelephone
        {
            get { return _contacttelephone; }
            set { _contacttelephone = value; }
        }
        private string _contactfax;
        /// <summary>
        /// 联系人传真
        /// </summary>
        public string contactfax
        {
            get { return _contactfax; }
            set { _contactfax = value; }
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
        private bool _ispricedispute = false;
        /// <summary>
        /// 是否价格争议 1是
        /// </summary>
        public bool ispricedispute
        {
            get { return _ispricedispute; }
            set { _ispricedispute = value; }
        }
        private decimal? _totalpriceonrequest;
        /// <summary>
        /// 争议总价
        /// </summary>
        public decimal? totalpriceonrequest
        {
            get { return _totalpriceonrequest; }
            set { _totalpriceonrequest = value; }
        }
        private decimal? _taxonrequest;
        /// <summary>
        /// 争议税费
        /// </summary>
        public decimal? taxonrequest
        {
            get { return _taxonrequest; }
            set { _taxonrequest = value; }
        }
        private decimal? _netpriceonrequest;
        /// <summary>
        /// 争议净值
        /// </summary>
        public decimal? netpriceonrequest
        {
            get { return _netpriceonrequest; }
            set { _netpriceonrequest = value; }
        }
        private decimal _pricedisputeamount = 0;
        /// <summary>
        /// 价格争议额度
        /// </summary>
        public decimal pricedisputeamount
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
        private byte? _dataorigintype;
        /// <summary>
        /// 数据来源，1-从询价添加,2-从委估对象添加的价格信息
        /// </summary>
        public byte? dataorigintype
        {
            get { return _dataorigintype; }
            set { _dataorigintype = value; }
        }

        /// <summary>
        /// 是否有效，用作删除标记
        /// </summary>
        public bool? valid { get; set; }


        private long _outentrustid = 0;
        /// <summary>
        /// 外部委托Id  
        /// </summary>
        public long outentrustid
        {
            get { return _outentrustid; }
            set { _outentrustid = value; }
        }
        private int? _outcompanyid;
        /// <summary>
        /// 外部companyid 
        /// </summary>
        public int? outcompanyid
        {
            get { return _outcompanyid; }
            set { _outcompanyid = value; }
        }
        private bool? _outcancel;
        /// <summary>
        /// 外部 是否撤销询价  
        /// </summary>
        public bool? outcancel
        {
            get { return _outcancel; }
            set { _outcancel = value; }
        }
        private long? _outobjectid;
        /// <summary>
        /// 外部  objectid
        /// </summary>
        public long? outobjectid
        {
            get { return _outobjectid; }
            set { _outobjectid = value; }
        }
        private string _revokeremark;
        /// <summary>
        /// 撤销 备注
        /// </summary>
        public string revokeremark
        {
            get { return _revokeremark; }
            set { _revokeremark = value; }
        }
        private decimal? _adjustprice;
        /// <summary>
        /// 调整后的价格
        /// </summary>
        public decimal? adjustprice
        {
            get { return _adjustprice; }
            set { _adjustprice = value; }
        }
        private decimal? _adjustamount;
        public decimal? adjustamount
        {
            get { return _adjustamount; }
            set { _adjustamount = value; }
        }
        public string adjustremark
        {
            get;
            set;
        }
        /// <summary>
        /// 纠错人  --新增  2015-06-09 潘锦发
        /// </summary>
        public string adjusters
        {
            get;
            set;
        }
        /// <summary>
        /// 纠错时间  --新增  2015-06-13 潘锦发
        /// </summary>
        public DateTime adjusttime
        {
            get;
            set;
        }  
        private int? _adjustworktodoid;
        /// <summary>
        /// 调价流程Id
        /// </summary>
        public int? adjustworktodoid
        {
            get { return _adjustworktodoid; }
            set { _adjustworktodoid = value; }
        }
        private decimal? _adjusttotalprice;
        /// <summary>
        /// 调价总价
        /// </summary>
        public decimal? adjusttotalprice
        {
            get { return _adjusttotalprice; }
            set { _adjusttotalprice = value; }
        }
        private decimal? _adjusttax;
        /// <summary>
        /// 调价税费
        /// </summary>
        public decimal? adjusttax
        {
            get { return _adjusttax; }
            set { _adjusttax = value; }
        }
        private decimal? _adjustnetprice;
        /// <summary>
        /// 调价净值
        /// </summary>
        public decimal? adjustnetprice
        {
            get { return _adjustnetprice; }
            set { _adjustnetprice = value; }
        }

        private decimal? _firstunitprice;
        /// <summary>
        /// 最开始估价师回的价格
        /// </summary>
        public decimal? firstunitprice
        {
            get { return _firstunitprice; }
            set { _firstunitprice = value; }
        }


        private string _source = "web";
        /// <summary>
        /// 来源
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _clientname;
        /// <summary>
        /// 委托方
        /// </summary>
        public string clientname
        {
            get { return _clientname; }
            set { _clientname = value; }
        }
        private string _clientphone;
        /// <summary>
        /// 委托方联系电话
        /// </summary>
        public string clientphone
        {
            get { return _clientphone; }
            set { _clientphone = value; }
        }
        private string _bankcontact;
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string bankcontact
        {
            get { return _bankcontact; }
            set { _bankcontact = value; }
        }
        private string _bankcontactphone;
        /// <summary>
        /// 业务联系人电话
        /// </summary>
        public string bankcontactphone
        {
            get { return _bankcontactphone; }
            set { _bankcontactphone = value; }
        }
        private string _clientcontactusername;
        /// <summary>
        /// 委托方联系人
        /// </summary>
        public string clientcontactusername
        {
            get { return _clientcontactusername; }
            set { _clientcontactusername = value; }
        }
        private bool _cbautocalculate = true;
        /// <summary>
        /// 是否联动计算
        /// </summary>
        public bool cbautocalculate
        {
            get { return _cbautocalculate; }
            set { _cbautocalculate = value; }
        }
        private int _queryassignuserid = 0;
        /// <summary>
        /// 询价分配人
        /// </summary>
        public int queryassignuserid
        {
            get { return _queryassignuserid; }
            set { _queryassignuserid = value; }
        }
        private string _priceremark;
        /// <summary>
        /// 价格说明
        /// </summary>
        public string priceremark
        {
            get { return _priceremark; }
            set { _priceremark = value; }
        }

        private string _saledepartment;
        /// <summary>
        /// 业务员所属部门
        /// </summary>
        public string saledepartment
        {
            get { return _saledepartment; }
            set { _saledepartment = value; }
        }

        private string _saledepartmentleader;
        /// <summary>
        /// 业务员所属部门负责人
        /// </summary>
        public string saledepartmentleader
        {
            get { return _saledepartmentleader; }
            set { _saledepartmentleader = value; }
        }

        private string _customerusername;
        /// <summary>
        /// 客户用户账号
        /// </summary>
        public string customerusername
        {
            get { return _customerusername; }
            set { _customerusername = value; }
        }
        private int? _historyid;
        /// <summary>
        /// 自动估价记录Id
        /// </summary>
        public int? historyid
        {
            get { return _historyid; }
            set { _historyid = value; }
        }
        private int? _autoprice;
        /// <summary>
        /// 自动估价单价
        /// </summary>
        public int? autoprice
        {
            get { return _autoprice; }
            set { _autoprice = value; }
        }
        private decimal? _autototalprice;
        /// <summary>
        /// 自动估价总价
        /// </summary>
        public decimal? autototalprice
        {
            get { return _autototalprice; }
            set { _autototalprice = value; }
        }
        private int? _customerid;
        /// <summary>
        /// 客户Id
        /// </summary>
        public int? customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }

        private string _banktype;
        /// <summary>
        /// 客户类型
        /// </summary>
        public string banktype
        {
            get { return _banktype; }
            set { _banktype = value; }
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
        private string _queryid="0";
        /// <summary>
        /// 询价编号
        /// </summary>
        public string queryid
        {
            get { return _queryid; }
            set { _queryid = value; }
        }
        private int? _cancelqueryuserid;
        /// <summary>
        /// 撤销人
        /// </summary>
        public int? cancelqueryuserid
        {
            get { return _cancelqueryuserid; }
            set { _cancelqueryuserid = value; }
        }
        private DateTime? _canceldate;
        /// <summary>
        /// 撤销时间
        /// </summary>
        public DateTime? canceldate
        {
            get { return _canceldate; }
            set { _canceldate = value; }
        }
        private decimal? _legalpayment;
        /// <summary>
        /// 法定优先受偿款
        /// </summary>
        public decimal? legalpayment
        {
            get { return _legalpayment; }
            set { _legalpayment = value; }
        }
        private decimal? _adjusthousetotalprice;
        /// <summary>
        /// 价格纠错评估总价
        /// </summary>
        public decimal? adjusthousetotalprice
        {
            get { return _adjusthousetotalprice; }
            set { _adjusthousetotalprice = value; }
        }
        private decimal? _housetotalpriceonrequest;
        /// <summary>
        /// 价格争议评估总价
        /// </summary>
        public decimal? housetotalpriceonrequest
        {
            get { return _housetotalpriceonrequest; }
            set { _housetotalpriceonrequest = value; }
        }
        private int? _departmentid;
        /// <summary>
        /// 业务部门
        /// </summary>
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
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
        /// 应补地价
        /// </summary>
        public decimal? shouldfilllandprice
        {
            get { return _shouldfilllandprice; }
            set { _shouldfilllandprice = value; }
        }
        private decimal? _liquidityvalue;
        /// <summary>
        /// 强制变现值
        /// </summary>
        public decimal? liquidityvalue
        {
            get { return _liquidityvalue; }
            set { _liquidityvalue = value; }
        }
        private decimal? _liquiditytaxvalue;
        /// <summary>
        /// 强制变现税费额
        /// </summary>
        public decimal? liquiditytaxvalue
        {
            get { return _liquiditytaxvalue; }
            set { _liquiditytaxvalue = value; }
        }
        private int? _lastopenuserid;
        /// <summary>
        /// 最后一次打开的用户
        /// </summary>
        public int? lastopenuserid
        {
            get { return _lastopenuserid; }
            set { _lastopenuserid = value; }
        }
        private DateTime? _lastopentime;
        /// <summary>
        /// 最后一次打开的时间
        /// </summary>
        public DateTime? lastopentime
        {
            get { return _lastopentime; }
            set { _lastopentime = value; }
        }

        private DateTime? _queryassigndate;
        /// <summary>
        /// 询价分配时间
        /// </summary>
        public DateTime? queryassigndate
        {
            get { return _queryassigndate; }
            set { _queryassigndate = value; }
        }

        private int? _stampstatecode;
        /// <summary>
        /// 盖章状态 10005
        /// </summary>
        public int? stampstatecode
        {
            get { return _stampstatecode; }
            set { _stampstatecode = value; }
        }
        private DateTime? _applystampdate;
        /// <summary>
        /// 盖章申请时间
        /// </summary>
        public DateTime? applystampdate
        {
            get { return _applystampdate; }
            set { _applystampdate = value; }
        }
        private DateTime? _stampdate;
        /// <summary>
        /// 盖章时间
        /// </summary>
        public DateTime? stampdate
        {
            get { return _stampdate; }
            set { _stampdate = value; }
        }
        private int? _stampuserid;
        /// <summary>
        /// 盖章人
        /// </summary>
        public int? stampuserid
        {
            get { return _stampuserid; }
            set { _stampuserid = value; }
        }
        private string _businesstypename;
        /// <summary>
        /// 技术团队名--20150615 潘锦发
        /// </summary>
        public string businesstypename
        {
            get { return _businesstypename; }
            set { _businesstypename = value; }
        }
        private int? _businesstypeid;
        /// <summary>
        /// 技术团队id--20150615 潘锦发
        /// </summary>
        public int? businesstypeid
        {
            get { return _businesstypeid; }
            set { _businesstypeid = value; }
        }
        private string _subcompanyname;
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string subcompanyname
        {
            get { return _subcompanyname; }
            set { _subcompanyname = value; }
        }
        private string _querytypename;
        /// <summary>
        /// 委估对象类型
        /// </summary>
        public string querytypename
        {
            get { return _querytypename; }
            set { _querytypename = value; }
        }
        private string _righttypename;
        /// <summary>
        /// 权属类型
        /// </summary>
        public string righttypename
        {
            get { return _righttypename; }
            set { _righttypename = value; }
        }
        private string _createusername;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string createusername
        {
            get { return _createusername; }
            set { _createusername = value; }
        }
        private string _statecodename;
        /// <summary>
        /// 状态名称
        /// </summary>
        public string statecodename
        {
            get { return _statecodename; }
            set { _statecodename = value; }
        }
        private string _cityname;
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname
        {
            get { return _cityname; }
            set { _cityname = value; }
        }
        private string _areaname;
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private string _salemanname;
        /// <summary>
        /// 业务员
        /// </summary>
        public string salemanname
        {
            get { return _salemanname; }
            set { _salemanname = value; }
        }
        private string _appraisersusername;
        /// <summary>
        /// 估价师
        /// </summary>
        public string appraisersusername
        {
            get { return _appraisersusername; }
            set { _appraisersusername = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

     
        private decimal? _landunitprice;
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
        }
        private decimal? _landtotalprice;
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? landtotalprice
        {
            get { return _landtotalprice; }
            set { _landtotalprice = value; }
        }
        private int? _loanpernum;
        /// <summary>
        /// 贷款成数
        /// </summary>
        public int? loanpernum
        {
            get { return _loanpernum; }
            set { _loanpernum = value; }
        }
        private decimal? _loanablenum;
        /// <summary>
        /// 可贷金额
        /// </summary>
        public decimal? loanablenum
        {
            get { return _loanablenum; }
            set { _loanablenum = value; }
        }
        private int? _reporttypecode;
        /// <summary>
        /// 报告类型，和评估目的一起
        /// </summary>
        public int? reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _assesstype;
        /// <summary>
        /// 评估目的，关联报告类型
        /// </summary>
        public string assesstype
        {
            get { return _assesstype; }
            set { _assesstype = value; }
        }

        private DateTime? _approvaldate;
        /// <summary>
        /// 回价审批时间  Alex 2016-09-09
        /// </summary>
        public DateTime? approvaldate
        {
            get { return _approvaldate; }
            set { _approvaldate = value; }
        }
        private DateTime? _adjustapprovaldate;
        /// <summary>
        /// 纠错审批时间 Alex 2016-09-09
        /// </summary>
        public DateTime? adjustapprovaldate
        {
            get { return _adjustapprovaldate; }
            set { _adjustapprovaldate = value; }
        }
        private string _wxopenid;
        /// <summary>
        /// 微信openid 记录游客询价记录
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }
    }
}