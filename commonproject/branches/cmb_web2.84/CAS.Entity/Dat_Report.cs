using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("dbo.DAT_Report")]
    public class DATReport : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
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
        public int qprojectid { get; set; }
        public long reportid { get; set; }
        private int _reporttypecode = 0;
        /// <summary>
        /// 报告类型：房地产，土地，资产
        /// </summary>
        public int reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        [SQLReadOnly]
        public string reporttypename { get; set; }
        private int _reportsubtypecode = 0;
        /// <summary>
        /// 报告细分类型
        /// </summary>
        public int reportsubtypecode
        {
            get { return _reportsubtypecode; }
            set { _reportsubtypecode = value; }
        }
        private int _housenumber = 1;
        /// <summary>
        /// 委估对象数量
        /// </summary>
        public int housenumber
        {
            get { return _housenumber; }
            set { _housenumber = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _fxtcompanyname;
        /// <summary>
        /// 评估机构名称
        /// </summary>
        public string fxtcompanyname
        {
            get { return _fxtcompanyname; }
            set { _fxtcompanyname = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 业务来源公司ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        [SQLReadOnly]
        public string companyname { get; set; }
        private int? _departmentid;
        /// <summary>
        /// 业务来源公司部门ID
        /// </summary>
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        [SQLReadOnly]
        public string departmentname { get; set; }
        private int? _clientid;
        /// <summary>
        /// 业务来源客户ID，关联fxtProject.dbo.Dat_Client
        /// </summary>
        public int? clientid
        {
            get { return _clientid; }
            set { _clientid = value; }
        }
        public string clientname { get; set; }
        private string _customername;
        /// <summary>
        /// 业务来源客户全称
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private string _projectaddress;
        /// <summary>
        /// 项目地址
        /// </summary>
        public string projectaddress
        {
            get { return _projectaddress; }
            set { _projectaddress = value; }
        }
        private string _projectnames;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectnames
        {
            get { return _projectnames; }
            set { _projectnames = value; }
        }
        private string _assessor1;
        /// <summary>
        /// 估价师1
        /// </summary>
        public string assessor1
        {
            get { return _assessor1; }
            set { _assessor1 = value; }
        }
        private string _assessorid1;
        /// <summary>
        /// 估价师Id1
        /// </summary>
        public string assessorid1
        {
            get { return _assessorid1; }
            set { _assessorid1 = value; }
        }
        private int _assessortype1 = 0;
        /// <summary>
        /// 估价师1类型
        /// </summary>
        public int assessortype1
        {
            get { return _assessortype1; }
            set { _assessortype1 = value; }
        }
        private string _assessornumber1;
        /// <summary>
        /// 估价师1注册号
        /// </summary>
        public string assessornumber1
        {
            get { return _assessornumber1; }
            set { _assessornumber1 = value; }
        }
        private string _assessor2;
        /// <summary>
        /// 估价师2
        /// </summary>
        public string assessor2
        {
            get { return _assessor2; }
            set { _assessor2 = value; }
        }
        private string _assessorid2;
        /// <summary>
        /// 估价师Id2
        /// </summary>
        public string assessorid2
        {
            get { return _assessorid2; }
            set { _assessorid2 = value; }
        }
        private int _assessortype2;
        /// <summary>
        /// 估价师2类型
        /// </summary>
        public int assessortype2
        {
            get { return _assessortype2; }
            set { _assessortype2 = value; }
        }
        private string _assessornumber2;
        /// <summary>
        /// 估价师2注册号
        /// </summary>
        public string assessornumber2
        {
            get { return _assessornumber2; }
            set { _assessornumber2 = value; }
        }
        private string _assessor3;
        /// <summary>
        /// 估价师3
        /// </summary>
        public string assessor3
        {
            get { return _assessor3; }
            set { _assessor3 = value; }
        }
        private string _assessorid3;
        /// <summary>
        /// 估价师Id3
        /// </summary>
        public string assessorid3
        {
            get { return _assessorid3; }
            set { _assessorid3 = value; }
        }
        private int? _assessortype3;
        /// <summary>
        /// 估价师3类型
        /// </summary>
        public int? assessortype3
        {
            get { return _assessortype3; }
            set { _assessortype3 = value; }
        }
        private string _assessornumber3;
        /// <summary>
        /// 估价师3注册号
        /// </summary>
        public string assessornumber3
        {
            get { return _assessornumber3; }
            set { _assessornumber3 = value; }
        }
        private DateTime _workingdatestart;
        /// <summary>
        /// 报告作业起始时间
        /// </summary>
        public DateTime workingdatestart
        {
            get { return _workingdatestart; }
            set { _workingdatestart = value; }
        }
        private DateTime _workingdateend;
        /// <summary>
        /// 报告作业结束时间
        /// </summary>
        public DateTime workingdateend
        {
            get { return _workingdateend; }
            set { _workingdateend = value; }
        }
        private DateTime _pricedate;
        /// <summary>
        /// 估价时点
        /// </summary>
        public DateTime pricedate
        {
            get { return _pricedate; }
            set { _pricedate = value; }
        }
        private int _assesstype;
        /// <summary>
        /// 评估目的
        /// </summary>
        public int assesstype
        {
            get { return _assesstype; }
            set { _assesstype = value; }
        }
        private string _assesstype1;
        /// <summary>
        /// 带报告使用方的评估目的
        /// </summary>
        public string assesstype1
        {
            get { return _assesstype1; }
            set { _assesstype1 = value; }
        }
        private int? _purposecode;
        /// <summary>
        /// 物业用途
        /// </summary>
        public int? purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private string _validity;
        /// <summary>
        /// 报告有效期
        /// </summary>
        public string validity
        {
            get { return _validity; }
            set { _validity = value; }
        }
        private DateTime _validitystart;
        /// <summary>
        /// 报告有效期开始日期
        /// </summary>
        public DateTime validitystart
        {
            get { return _validitystart; }
            set { _validitystart = value; }
        }
        private DateTime _validityend;
        /// <summary>
        /// 报告有效期结束日期
        /// </summary>
        public DateTime validityend
        {
            get { return _validityend; }
            set { _validityend = value; }
        }
        private int _moneyunitcode = 0;
        /// <summary>
        /// 币种
        /// </summary>
        public int moneyunitcode
        {
            get { return _moneyunitcode; }
            set { _moneyunitcode = value; }
        }
        private decimal _totalprice;
        /// <summary>
        /// 总价(多套物业之和)
        /// </summary>
        public decimal totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private string _totalpricedx;
        /// <summary>
        /// 总价大写(多套物业之和)
        /// </summary>
        public string totalpricedx
        {
            get { return _totalpricedx; }
            set { _totalpricedx = value; }
        }
        private decimal _buildingarea;
        /// <summary>
        /// 总建筑面积(多套物业之和)
        /// </summary>
        public decimal buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _tax;
        /// <summary>
        /// 税费(多套物业之和)
        /// </summary>
        public decimal? tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal _netprice;
        /// <summary>
        /// 净值(多套物业之和)
        /// </summary>
        public decimal netprice
        {
            get { return _netprice; }
            set { _netprice = value; }
        }
        private string _legalman;
        /// <summary>
        /// 法人代表
        /// </summary>
        public string legalman
        {
            get { return _legalman; }
            set { _legalman = value; }
        }
        private string _fxtcompanyaddress;
        /// <summary>
        /// 评估机构地址
        /// </summary>
        public string fxtcompanyaddress
        {
            get { return _fxtcompanyaddress; }
            set { _fxtcompanyaddress = value; }
        }
        private string _fxtcompanyaptitude;
        /// <summary>
        /// 评估机构资质
        /// </summary>
        public string fxtcompanyaptitude
        {
            get { return _fxtcompanyaptitude; }
            set { _fxtcompanyaptitude = value; }
        }
        private string _fxtcompanyphone;
        /// <summary>
        /// 评估机构电话
        /// </summary>
        public string fxtcompanyphone
        {
            get { return _fxtcompanyphone; }
            set { _fxtcompanyphone = value; }
        }
        private DateTime _reportdate;
        /// <summary>
        /// 报告日期
        /// </summary>
        public DateTime reportdate
        {
            get { return _reportdate; }
            set { _reportdate = value; }
        }
        private string _writer;
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public string writer
        {
            get { return _writer; }
            set { _writer = value; }
        }
        private string _writerid;
        /// <summary>
        /// 报告撰写人ID
        /// </summary>
        public string writerid
        {
            get { return _writerid; }
            set { _writerid = value; }
        }
        private DateTime _writedate;
        /// <summary>
        /// 报告撰写时间
        /// </summary>
        public DateTime writedate
        {
            get { return _writedate; }
            set { _writedate = value; }
        }
        private string _salemanid;
        /// <summary>
        /// 评估机构业务员ID
        /// </summary>
        public string salemanid
        {
            get { return _salemanid; }
            set { _salemanid = value; }
        }
        private string _saleman;
        /// <summary>
        /// 评估机构业务员
        /// </summary>
        public string saleman
        {
            get { return _saleman; }
            set { _saleman = value; }
        }
        private string _surveyuserid;
        /// <summary>
        /// 查勘员Id
        /// </summary>
        public string surveyuserid
        {
            get { return _surveyuserid; }
            set { _surveyuserid = value; }
        }
        private string _surveyuser;
        /// <summary>
        /// 查勘员
        /// </summary>
        public string surveyuser
        {
            get { return _surveyuser; }
            set { _surveyuser = value; }
        }
        private DateTime? _surveydate;
        /// <summary>
        /// 查勘时间
        /// </summary>
        public DateTime? surveydate
        {
            get { return _surveydate; }
            set { _surveydate = value; }
        }
        private string _marketdetial;
        /// <summary>
        /// 市场背景分析
        /// </summary>
        public string marketdetial
        {
            get { return _marketdetial; }
            set { _marketdetial = value; }
        }
        private int? _reportstatecode;
        /// <summary>
        /// 报告状态ID(系统流程)
        /// </summary>
        public int? reportstatecode
        {
            get { return _reportstatecode; }
            set { _reportstatecode = value; }
        }
        [SQLReadOnly]
        public string reportstatecodename { get; set; }
        private string _reportstate;
        /// <summary>
        /// 报告状态(对外显示)
        /// </summary>
        public string reportstate
        {
            get { return _reportstate; }
            set { _reportstate = value; }
        }
        private byte? _reportnumber;
        /// <summary>
        /// 报告份数
        /// </summary>
        public byte? reportnumber
        {
            get { return _reportnumber; }
            set { _reportnumber = value; }
        }
        private int? _chargetypecode;
        /// <summary>
        /// 收款情况ID
        /// </summary>
        public int? chargetypecode
        {
            get { return _chargetypecode; }
            set { _chargetypecode = value; }
        }
        private decimal? _pricetitular;
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? pricetitular
        {
            get { return _pricetitular; }
            set { _pricetitular = value; }
        }
        private decimal? _pricereally;
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal? pricereally
        {
            get { return _pricereally; }
            set { _pricereally = value; }
        }
        private int _reportcancel = 0;
        /// <summary>
        /// 报告撤销
        /// </summary>
        public int reportcancel
        {
            get { return _reportcancel; }
            set { _reportcancel = value; }
        }
        private string _reportcanceluser="";
        /// <summary>
        /// 报告撤销人
        /// </summary>
        public string reportcanceluser
        {
            get { return _reportcanceluser; }
            set { _reportcanceluser = value; }
        }
        private DateTime? _reportcanceldate ;
        /// <summary>
        /// 报告撤销时间
        /// </summary>
        public DateTime? reportcanceldate
        {
            get { return _reportcanceldate; }
            set { _reportcanceldate = value; }
        }
        private string _reportcancelremark ;
        /// <summary>
        /// 报告撤销原因
        /// </summary>
        public string reportcancelremark
        {
            get { return _reportcancelremark; }
            set { _reportcancelremark = value; }
        }
        private byte _isreturn = ((0));
        /// <summary>
        /// 是否退件
        /// </summary>
        public byte isreturn
        {
            get { return _isreturn; }
            set { _isreturn = value; }
        }
        private string _reportremark;
        /// <summary>
        /// 报告备注
        /// </summary>
        public string reportremark
        {
            get { return _reportremark; }
            set { _reportremark = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _updatetime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? updatetime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }
        private string _updateuser;
        /// <summary>
        /// 修改用户
        /// </summary>
        public string updateuser
        {
            get { return _updateuser; }
            set { _updateuser = value; }
        }
        private byte _valid = ((1));
        /// <summary>
        /// 是否有效
        /// </summary>
        public byte valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime? _clientsubmitdate;
        /// <summary>
        /// 客户委托时间
        /// </summary>
        public DateTime? clientsubmitdate
        {
            get { return _clientsubmitdate; }
            set { _clientsubmitdate = value; }
        }
        private DateTime? _clientwishenddate;
        /// <summary>
        /// 客户希望完成时间
        /// </summary>
        public DateTime? clientwishenddate
        {
            get { return _clientwishenddate; }
            set { _clientwishenddate = value; }
        }
        private int _clientwishsubmittype = 0;
        /// <summary>
        /// 客户希望报告提交方式
        /// </summary>
        public int clientwishsubmittype
        {
            get { return _clientwishsubmittype; }
            set { _clientwishsubmittype = value; }
        }
        public int mastcompanyid { get; set; }
        public int biztype { get; set; }

        /// <summary>
        /// 估价师1类型
        /// </summary>
        [SQLReadOnly]
        public string assessortype1name { get; set; }

        /// <summary>
        /// 估价师2类型
        /// </summary>
        [SQLReadOnly]
        public string assessortype1name2 { get; set; }
      
        /// <summary>
        /// 估价师3类型
        /// </summary>
        [SQLReadOnly]
        public string assessortype1name3 { get; set; }

        /// <summary>
        /// 报告发送次数
        /// </summary>
        [SQLReadOnly]
        public int sendcount { get; set; }

        /// <summary>
        /// 备案次数
        /// </summary>
        [SQLReadOnly]
        public int saveupcount { get; set; }

        /// <summary>
        /// 业务来源
        /// </summary>
        [SQLReadOnly]
        public string bankdepname { get; set; }

        /// <summary>
        /// 是否发送报告
        /// </summary>
        [SQLReadOnly]
        public string issendtext
        {
            get {
                return 0 < sendcount ? "是" : "否";
            }
        }
        /// <summary>
        /// 是否备案
        /// </summary>
        [SQLReadOnly]
        public string issaveuptext
        {
            get
            {
                return 0 < saveupcount ? "是" : "否";
            }
        }
        ///// <summary>
        ///// 登记流程
        ///// <remarks>subclass</remarks>
        ///// </summary>
        //[SQLReadOnly]
       // public SYSFlowLog flowlog;
    }
}