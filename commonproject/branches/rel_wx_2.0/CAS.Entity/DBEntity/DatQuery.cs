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
        private decimal _unitprice = 0.00M;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private decimal _totalprice = 0.00M;
        /// <summary>
        /// 总价
        /// </summary>
        public decimal totalprice
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
        public string queryusername { get; set; }
        private int? _queryuserid;
        /// <summary>
        /// 询价人ID
        /// </summary>
        public int? queryuserid
        {
            get { return _queryuserid; }
            set { _queryuserid = value; }
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
        /// 客户经理用户Id
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
        private int? _adjustworktodoid;
        /// <summary>
        /// 调价流程Id
        /// </summary>
        public int? adjustworktodoid
        {
            get { return _adjustworktodoid; }
            set { _adjustworktodoid = value; }
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
        /// 最开始估价师回的价格
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}