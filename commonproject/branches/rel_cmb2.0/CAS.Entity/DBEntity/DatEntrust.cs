using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_Entrust")]
	public class DatEntrust : BaseTO
	{
		private long _eid;
		/// <summary>
		/// 委托报告
		/// </summary>
		[SQLField("eid", EnumDBFieldUsage.PrimaryKey, true)]
		public long eid
		{
			get{ return _eid;}
			set{ _eid=value;}
		}
		private int _cityid;
		/// <summary>
		/// 城市ID
		/// </summary>
		public int cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private int _fxtcompanyid;
		/// <summary>
		/// 评估机构ID(业务公司)
		/// </summary>
		public int fxtcompanyid
		{
			get{ return _fxtcompanyid;}
			set{ _fxtcompanyid=value;}
		}
		private int _subcompanyid;
		/// <summary>
		/// 分支机构ID
		/// </summary>
		public int subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private string _entrustno;
		/// <summary>
		/// 委托单编号(手动填写的)
		/// </summary>
		public string entrustno
		{
			get{ return _entrustno;}
			set{ _entrustno=value;}
		}
		private long _entrustid;
		/// <summary>
		/// 流水号,业务编号(自动生成的,其他表关联)2013030001
		/// </summary>
		public long entrustid
		{
			get{ return _entrustid;}
			set{ _entrustid=value;}
		}
		private string _projectname;
		/// <summary>
		/// 项目名称
		/// </summary>
		public string projectname
		{
			get{ return _projectname;}
			set{ _projectname=value;}
		}
		private bool _entrustyp = false;
		/// <summary>
		/// 是否委托预评报告 0：否   1：是
		/// </summary>
		public bool entrustyp
		{
			get{ return _entrustyp;}
			set{ _entrustyp=value;}
		}
		private int? _bankid;
		/// <summary>
		/// 银行ID
		/// </summary>
		public int? bankid
		{
			get{ return _bankid;}
			set{ _bankid=value;}
		}
		private string _bankbranchname;
		/// <summary>
		/// 分行名称
		/// </summary>
		public string bankbranchname
		{
			get{ return _bankbranchname;}
			set{ _bankbranchname=value;}
		}
		private int? _bankbranchid;
		/// <summary>
		/// 分行ID
		/// </summary>
		public int? bankbranchid
		{
			get{ return _bankbranchid;}
			set{ _bankbranchid=value;}
		}
		private string _banksubbranchname;
		/// <summary>
		/// 支行名称
		/// </summary>
		public string banksubbranchname
		{
			get{ return _banksubbranchname;}
			set{ _banksubbranchname=value;}
		}
		private int? _banksubbranchid;
		/// <summary>
		/// 支行ID
		/// </summary>
		public int? banksubbranchid
		{
			get{ return _banksubbranchid;}
			set{ _banksubbranchid=value;}
		}
		private int? _bankuserid;
		/// <summary>
		/// 银行客户经理ID
		/// </summary>
		public int? bankuserid
		{
			get{ return _bankuserid;}
			set{ _bankuserid=value;}
		}
		private string _bankusername;
		/// <summary>
		/// 银行客户经理名称
		/// </summary>
		public string bankusername
		{
			get{ return _bankusername;}
			set{ _bankusername=value;}
		}
		private string _bankuserphone;
		/// <summary>
		/// 银行客户经理联系电话
		/// </summary>
		public string bankuserphone
		{
			get{ return _bankuserphone;}
			set{ _bankuserphone=value;}
		}
		private string _bankuserfax;
		/// <summary>
		/// 联系人传真
		/// </summary>
		public string bankuserfax
		{
			get{ return _bankuserfax;}
			set{ _bankuserfax=value;}
		}
		private string _clientname;
		/// <summary>
		/// 委托人，一般是产权人
		/// </summary>
		public string clientname
		{
			get{ return _clientname;}
			set{ _clientname=value;}
		}
		private string _clientphone;
		/// <summary>
		/// 委托人联系电话
		/// </summary>
		public string clientphone
		{
			get{ return _clientphone;}
			set{ _clientphone=value;}
		}
		private int? _reportnumber;
		/// <summary>
		/// 报告份数
		/// </summary>
		public int? reportnumber
		{
			get{ return _reportnumber;}
			set{ _reportnumber=value;}
		}
		private int _submittype = 1017001;
		/// <summary>
		/// 报告投递方式
		/// </summary>
		public int submittype
		{
			get{ return _submittype;}
			set{ _submittype=value;}
		}
		private string _remarks;
		public string remarks
		{
			get{ return _remarks;}
			set{ _remarks=value;}
		}
		private DateTime _createdate = DateTime.Now;
		public DateTime createdate
		{
			get{ return _createdate;}
			set{ _createdate=value;}
		}
		private int _createuserid;
		public int createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}
		private int _biztype = 200104001;
		/// <summary>
		/// 委托类型，对公、个人
		/// </summary>
		public int biztype
		{
			get{ return _biztype;}
			set{ _biztype=value;}
		}
		private int _systypecode = 1003013;
		/// <summary>
		/// 系统类型
		/// </summary>
		public int systypecode
		{
			get{ return _systypecode;}
			set{ _systypecode=value;}
		}
		private bool _entrustreport = false;
		/// <summary>
		/// 是否委托正式报告(是否出报告) 0：否   1：是
		/// </summary>
		public bool entrustreport
		{
			get{ return _entrustreport;}
			set{ _entrustreport=value;}
		}
		private int _statecode = 0;
		/// <summary>
		/// 业务状态(待提交、待分配、处理中、已完成)
		/// </summary>
		public int statecode
		{
			get{ return _statecode;}
			set{ _statecode=value;}
		}
		private DateTime? _submitdate;
		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime? submitdate
		{
			get{ return _submitdate;}
			set{ _submitdate=value;}
		}
		private string _customercompanyfullname;
		/// <summary>
		/// 客户机构全称
		/// </summary>
		public string customercompanyfullname
		{
			get{ return _customercompanyfullname;}
			set{ _customercompanyfullname=value;}
		}
        private bool _ischargemonthly = false;
        /// <summary>
        /// 是否月结
        /// </summary>
        public bool ischargemonthly
        {
            get { return _ischargemonthly; }
            set { _ischargemonthly = value; }
        }

		private int _businesstypeid;
		/// <summary>
		/// 业务类型ID
		/// </summary>
		public int businesstypeid
		{
			get{ return _businesstypeid;}
			set{ _businesstypeid=value;}
		}
		private int? _submituserid;
		/// <summary>
		/// 提交人
		/// </summary>
		public int? submituserid
		{
			get{ return _submituserid;}
			set{ _submituserid=value;}
		}
		private DateTime? _assigndate;
		/// <summary>
		/// 分配时间
		/// </summary>
		public DateTime? assigndate
		{
			get{ return _assigndate;}
			set{ _assigndate=value;}
		}
		private int? _assignuserid;
		/// <summary>
		/// 分配人
		/// </summary>
		public int? assignuserid
		{
			get{ return _assignuserid;}
			set{ _assignuserid=value;}
		}
		private int? _reporttypecode;
		/// <summary>
		/// 房地产/土地/资产
		/// </summary>
		public int? reporttypecode
		{
			get{ return _reporttypecode;}
			set{ _reporttypecode=value;}
		}
		private string _assesstype;
		/// <summary>
		/// 评估目的
		/// </summary>
		public string assesstype
		{
			get{ return _assesstype;}
			set{ _assesstype=value;}
		}
		private DateTime? _reportwantdate;
		/// <summary>
		/// 要求报告出具时间
		/// </summary>
		public DateTime? reportwantdate
		{
			get{ return _reportwantdate;}
			set{ _reportwantdate=value;}
		}
		private int? _appraisersuserid;
		/// <summary>
		/// 估价师
		/// </summary>
		public int? appraisersuserid
		{
			get{ return _appraisersuserid;}
			set{ _appraisersuserid=value;}
		}
		private int? _businessuserid;
		/// <summary>
		/// 业务员
		/// </summary>
		public int? businessuserid
		{
			get{ return _businessuserid;}
			set{ _businessuserid=value;}
		}
        /// <summary>
        /// 是否有效，用作删除标记
        /// </summary>
        public bool? valid { get; set; }

        /// <summary>
        /// 收费类型
        /// </summary>
        public string chargetype { get; set; }
        private string _clientaddress;
        public string clientaddress
        {
            get { return _clientaddress; }
            set { _clientaddress = value; }
        }
        private string _clientshenfenzheng;
        public string clientshenfenzheng
        {
            get { return _clientshenfenzheng; }
            set { _clientshenfenzheng = value; }
        }
        private string _clientlegalperson;
        public string clientlegalperson
        {
            get { return _clientlegalperson; }
            set { _clientlegalperson = value; }
        }

        private string _housetype;
        /// <summary>
        /// 住宅类型   非住宅 ，非普通住宅 ， 普通住宅
        /// </summary>
        public string housetype
        {
            get { return _housetype; }
            set { _housetype = value; }
        }
        private int? _ownertype;
        /// <summary>
        /// 证载权利人
        /// </summary>
        public int? ownertype
        {
            get { return _ownertype; }
            set { _ownertype = value; }
        }

        private int _cancelentrustuserid = 0;
        /// <summary>
        /// 撤销人
        /// </summary>
        public int cancelentrustuserid
        {
            get { return _cancelentrustuserid; }
            set { _cancelentrustuserid = value; }
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

        private int _provinceid = 0;
        /// <summary>
        /// 省份
        /// </summary>
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private int _areaid = 0;
        /// <summary>
        /// 城市
        /// </summary>
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

        private int _departmendid = 0;
        /// <summary>
        /// 部门id
        /// </summary>
        public int departmendid
        {
            get { return _departmendid; }
            set { _departmendid = value; }
        }

        public string projectaddress { get; set; }

        private string _surveyuserid;
        /// <summary>
        /// 查勘员ID集合
        /// </summary>
        public string surveyuserid
        {
            get { return _surveyuserid; }
            set { _surveyuserid = value; }
        }
        private string _surveyuser;
        /// <summary>
        /// 查勘员名称
        /// </summary>
        public string surveyuser
        {
            get { return _surveyuser; }
            set { _surveyuser = value; }
        }
        private int _surveystate = 0;
        /// <summary>
        /// 查勘状态
        /// </summary>
        public int surveystate
        {
            get { return _surveystate; }
            set { _surveystate = value; }
        }
        private DateTime? _surveybegindate;
        /// <summary>
        /// 查勘开始时间
        /// </summary>
        public DateTime? surveybegindate
        {
            get { return _surveybegindate; }
            set { _surveybegindate = value; }
        }

        private DateTime? _surveyenddate;
        /// <summary>
        /// 查勘开始时间
        /// </summary>
        public DateTime? surveyenddate
        {
            get { return _surveyenddate; }
            set { _surveyenddate = value; }
        }

        private string _clientcontact;
        /// <summary>
        /// 委托方联系人
        /// </summary>
        public string clientcontact
        {
            get { return _clientcontact; }
            set { _clientcontact = value; }
        }

	}
}