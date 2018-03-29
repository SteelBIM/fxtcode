using System;
using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_Object")]
	public class DatObject : BaseTO
	{
		private long _objectid;
		/// <summary>
		/// 委估对象ID
		/// </summary>
		[SQLField("objectid", EnumDBFieldUsage.PrimaryKey, true)]
		public long objectid
		{
			get{ return _objectid;}
			set{ _objectid=value;}
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
		/// 评估机构ID
		/// </summary>
		public int fxtcompanyid
		{
			get{ return _fxtcompanyid;}
			set{ _fxtcompanyid=value;}
		}
		private int? _subcompanyid;
		/// <summary>
		/// 分支机构ID
		/// </summary>
		public int? subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private string _fullname;
		/// <summary>
		/// 委估对象全称
		/// </summary>
		public string fullname
		{
			get{ return _fullname;}
			set{ _fullname=value;}
		}
		private string _address = "";
		/// <summary>
		/// 地址
		/// </summary>
		public string address
		{
			get{ return _address;}
			set{ _address=value;}
		}
		private string _buildingarea;
		/// <summary>
		/// 建筑面积
		/// </summary>
        public string buildingarea
		{
			get{ return _buildingarea;}
			set{ _buildingarea=value;}
		}
		private int _typecode;
		/// <summary>
		/// 委估对象类型1031001
		/// </summary>
		public int typecode
		{
			get{ return _typecode;}
			set{ _typecode=value;}
		}
		private int _areaid;
		/// <summary>
		/// 行政区
		/// </summary>
		public int areaid
		{
			get{ return _areaid;}
			set{ _areaid=value;}
		}
        private int? _subareaid;
        /// <summary>
        /// 片区
        /// </summary>
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
		private int _ownertypecode = 0;
		/// <summary>
		/// 产权200105
		/// </summary>
		public int ownertypecode
		{
			get{ return _ownertypecode;}
			set{ _ownertypecode=value;}
		}
		private string _remark = "";
		/// <summary>
		/// 备注
		/// </summary>
		public string remark
		{
			get{ return _remark;}
			set{ _remark=value;}
		}
		private DateTime _createdate = DateTime.Now;
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime createdate
		{
			get{ return _createdate;}
			set{ _createdate=value;}
		}
		private int? _createuserid;
		/// <summary>
		/// 创建人
		/// </summary>
		public int? createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}
		private DateTime _savedate = DateTime.Now;
		/// <summary>
		/// 最后修改时间
		/// </summary>
		public DateTime savedate
		{
			get{ return _savedate;}
			set{ _savedate=value;}
		}
		private int? _saveuserid;
		/// <summary>
		/// 最后修改人
		/// </summary>
		public int? saveuserid
		{
			get{ return _saveuserid;}
			set{ _saveuserid=value;}
		}
		private int _valid = 1;
		/// <summary>
		/// 是否有效
		/// </summary>
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
		private long _entrustid = 0;
		/// <summary>
		/// 业务编号，委托编号
		/// </summary>
		public long entrustid
		{
			get{ return _entrustid;}
			set{ _entrustid=value;}
		}
		private long _mathfileid = 0;
		/// <summary>
		/// 测算表文件ID
		/// </summary>
		public long mathfileid
		{
			get{ return _mathfileid;}
			set{ _mathfileid=value;}
		}
		private string _owner;
		/// <summary>
		/// 权利人
		/// </summary>
		public string owner
		{
			get{ return _owner;}
			set{ _owner=value;}
		}
        private string _newaddflag;
        /// <summary>
        /// 新增委估对象标识
        /// </summary>
        public string newaddflag
        {
            get { return _newaddflag; }
            set { _newaddflag = value; }
        }
        private int? _thirdpartyid;
        /// <summary>
        /// 第三方公司ID 
        /// </summary>
        public int? thirdpartyid
        {
            get { return _thirdpartyid; }
            set { _thirdpartyid = value; }
        }
        private long? _sid;
        /// <summary>
        /// 第三方查勘sid
        /// </summary>
        public long? sid
        {
            get { return _sid; }
            set { _sid = value; }
        }
        private string _housecertno;
        /// <summary>
        /// 房产证号
        /// </summary>
        public string housecertno
        {
            get { return _housecertno; }
            set { _housecertno = value; }
        }
        private DateTime? _housecertdate;
        /// <summary>
        /// 房产证登记日期
        /// </summary>
        public DateTime? housecertdate
        {
            get { return _housecertdate; }
            set { _housecertdate = value; }
        }
        private decimal? _registrationprice;
        /// <summary>
        /// 登记价
        /// </summary>
        public decimal? registrationprice
        {
            get { return _registrationprice; }
            set { _registrationprice = value; }
        }
        private decimal? _housetotalprice;
        /// <summary>
        /// 住宅评估总值
        /// </summary>
        public decimal? housetotalprice
        {
            get { return _housetotalprice; }
            set { _housetotalprice = value; }
        }
        private string _landusecert;
        /// <summary>
        /// 土地使用权证
        /// </summary>
        public string landusecert
        {
            get { return _landusecert; }
            set { _landusecert = value; }
        }
        private int? _landusetype;
        /// <summary>
        /// 土地使用权类型
        /// </summary>
        public int? landusetype
        {
            get { return _landusetype; }
            set { _landusetype = value; }
        }
        private string _landusearea;
        /// <summary>
        /// 土地使用权面积
        /// </summary>
        public string landusearea
        {
            get { return _landusearea; }
            set { _landusearea = value; }
        }
        private string _landuseperson;
        /// <summary>
        /// 土地使用权人
        /// </summary>
        public string landuseperson
        {
            get { return _landuseperson; }
            set { _landuseperson = value; }
        }
        private string _landusecertaddress;
        /// <summary>
        /// 土地证载地址
        /// </summary>
        public string landusecertaddress
        {
            get { return _landusecertaddress; }
            set { _landusecertaddress = value; }
        }
        private DateTime? _landusecertdate;
        /// <summary>
        /// 土地证发证日期
        /// </summary>
        public DateTime? landusecertdate
        {
            get { return _landusecertdate; }
            set { _landusecertdate = value; }
        }
        private string _landusecertdepartment;
        /// <summary>
        /// 土地证发证机关
        /// </summary>
        public string landusecertdepartment
        {
            get { return _landusecertdepartment; }
            set { _landusecertdepartment = value; }
        }
        private string _queryid;
        /// <summary>
        /// 询价编号
        /// </summary>
        public string queryid
        {
            get { return _queryid; }
            set { _queryid = value; }
        }
        private DateTime? _buydate;
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime? buydate
        {
            get { return _buydate; }
            set { _buydate = value; }
        }
        private int? _errordatamarking = 0;
        /// <summary>
        /// 错误数据标识 （0:正常数据 1:错误数据）
        /// </summary>
        public int? errordatamarking
        {
            get { return _errordatamarking; }
            set { _errordatamarking = value; }
        }
        private string _backgroundanalysisids;
        public string backgroundanalysisids
        {
            get { return _backgroundanalysisids; }
            set { _backgroundanalysisids = value; }
        }
        private string _locationfactors;
        /// <summary>
        /// 区位因素分析
        /// </summary>
        public string locationfactors
        {
            get { return _locationfactors; }
            set { _locationfactors = value; }
        }
        private string _valuationmethodstext;
        /// <summary>
        /// 技术报告中的估价方法
        /// </summary>
        public string valuationmethodstext
        {
            get { return _valuationmethodstext; }
            set { _valuationmethodstext = value; }
        }
        private string _valuationmethodsids;
        /// <summary>
        /// 估价方法所选择的模板IDS
        /// </summary>
        public string valuationmethodsids
        {
            get { return _valuationmethodsids; }
            set { _valuationmethodsids = value; }
        }
        private string _bestdescribedtext;
        /// <summary>
        /// 最佳使用方法描述
        /// </summary>
        public string bestdescribedtext
        {
            get { return _bestdescribedtext; }
            set { _bestdescribedtext = value; }
        }
        private string _bestdescribedids;
        /// <summary>
        /// 最佳使用方法使用的模板
        /// </summary>
        public string bestdescribedids
        {
            get { return _bestdescribedids; }
            set { _bestdescribedids = value; }
        }
        private string _marketbackgroundids;
        public string marketbackgroundids
        {
            get { return _marketbackgroundids; }
            set { _marketbackgroundids = value; }
        }
        private string _marketbackgroundtext;
        /// <summary>
        /// 市场背景分析
        /// </summary>
        public string marketbackgroundtext
        {
            get { return _marketbackgroundtext; }
            set { _marketbackgroundtext = value; }
        }
        private decimal? _normalprice;
        /// <summary>
        /// 正常估价单价
        /// </summary>
        public decimal? normalprice
        {
            get { return _normalprice; }
            set { _normalprice = value; }
        }
        private int _surveystate;
        /// <summary>
        /// 查勘状态
        /// </summary>
        public int surveystate
        {
            get { return _surveystate; }
            set { _surveystate = value; }
        }
        private string _owneridcard;
        /// <summary>
        /// 产权人身份证
        /// </summary>
        public string owneridcard
        {
            get { return _owneridcard; }
            set { _owneridcard = value; }
        }
        private string _ownerphone;
        /// <summary>
        /// 产权人联系电话
        /// </summary>
        public string ownerphone
        {
            get { return _ownerphone; }
            set { _ownerphone = value; }
        }
        private string _rightpercent;
        /// <summary>
        /// 所有权比例
        /// </summary>
        public string rightpercent
        {
            get { return _rightpercent; }
            set { _rightpercent = value; }
        }
        private string _ownercontactsphone;
        /// <summary>
        /// 产权人联系人电话
        /// </summary>
        public string ownercontactsphone
        {
            get { return _ownercontactsphone; }
            set { _ownercontactsphone = value; }
        }
        private string _ownercontactsrelation;
        /// <summary>
        /// 产权人联系人关系
        /// </summary>
        public string ownercontactsrelation
        {
            get { return _ownercontactsrelation; }
            set { _ownercontactsrelation = value; }
        }
        private string _landarea;
        /// <summary>
        /// 土地面积
        /// </summary>
        public string landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
	}

    public class ObjectBusiness
    {
        private int _statecode;
        /// <summary>
        /// 状态
        /// </summary>
        public int statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        private int _assignstate;
        /// <summary>
        /// 转交状态
        /// </summary>
        public int assignstate
        {
            get { return _assignstate; }
            set { _assignstate = value; }
        }
        private int _writeruserid;
        /// <summary>
        /// 撰写人
        /// </summary>
        public int writeruserid
        {
            get { return _writeruserid; }
            set { _writeruserid = value; }
        }
        private string _rassistedwriterids;
        /// <summary>
        /// 报告辅助撰写人
        /// </summary>
        public string rassistedwriterids
        {
            get { return _rassistedwriterids; }
            set { _rassistedwriterids = value; }
        }

        
        private string _shenpiuserlist;
        /// <summary>
        /// 报告审批人
        /// </summary>
        public string shenpiuserlist 
        {
            get { return _shenpiuserlist; }
            set { _shenpiuserlist = value; }
        }
    }
}