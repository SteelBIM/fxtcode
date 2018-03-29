using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_YP")]
	public class DatYP : BaseTO
	{
		private long _id;
		/// <summary>
		/// 预估报告信息
		/// </summary>
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public long id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int _subcompanyid;
		public int subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private string _reportsubtype = "";
		/// <summary>
		/// 预估报告细分类型((1007001))
		/// </summary>
		public string reportsubtype
		{
			get{ return _reportsubtype;}
			set{ _reportsubtype=value;}
		}
		private string _ypnumber;
		/// <summary>
		/// 预评编号
		/// </summary>
		public string ypnumber
		{
			get{ return _ypnumber;}
			set{ _ypnumber=value;}
		}
		private string _customername;
		/// <summary>
		/// 使用方
		/// </summary>
		public string customername
		{
			get{ return _customername;}
			set{ _customername=value;}
		}
		private decimal? _totalprice;
		/// <summary>
        /// 评估总价(住房+土地+附属)
		/// </summary>
		public decimal? totalprice
		{
			get{ return _totalprice;}
			set{ _totalprice=value;}
		}
		private DateTime? _ypdate;
		/// <summary>
		/// 预评时间
		/// </summary>
		public DateTime? ypdate
		{
			get{ return _ypdate;}
			set{ _ypdate=value;}
		}
		private string _overdate;
		/// <summary>
		/// 有效期
		/// </summary>
		public string overdate
		{
			get{ return _overdate;}
			set{ _overdate=value;}
		}
        private int _statecode;
		/// <summary>
		/// 状态
		/// </summary>
		public int statecode
		{
			get{ return _statecode;}
			set{ _statecode=value;}
		}
		private int? _createuserid;
		public int? createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}
		private DateTime _createdate = DateTime.Now;
		public DateTime createdate
		{
			get{ return _createdate;}
			set{ _createdate=value;}
		}
		private long _entrustid = 0;
		/// <summary>
		/// 业务编号,从委托表来
		/// </summary>
		public long entrustid
		{
			get{ return _entrustid;}
			set{ _entrustid=value;}
		}
		private bool _needcharge = false;
		/// <summary>
		/// 是否收费预评
		/// </summary>
		public bool needcharge
		{
			get{ return _needcharge;}
			set{ _needcharge=value;}
		}
		private int _approvalstatus = 1;
		/// <summary>
		/// 审批状态 1未参与审批 2正在审批中 3审批完成
		/// </summary>
		public int approvalstatus
		{
			get{ return _approvalstatus;}
			set{ _approvalstatus=value;}
		}
		private int? _worktodoid;
		public int? worktodoid
		{
			get{ return _worktodoid;}
			set{ _worktodoid=value;}
		}
		private bool _performancedependent = false;
		/// <summary>
		/// 是否参与业绩统计
		/// </summary>
		public bool performancedependent
		{
			get{ return _performancedependent;}
			set{ _performancedependent=value;}
		}
		private bool _ispricedispute = false;
		/// <summary>
		/// 是否价格争议 1是
		/// </summary>
		public bool ispricedispute
		{
			get{ return _ispricedispute;}
			set{ _ispricedispute=value;}
		}
        private int _pricedisputeamount = 0;
		/// <summary>
		/// 价格争议额度
		/// </summary>
        public int pricedisputeamount
		{
			get{ return _pricedisputeamount;}
			set{ _pricedisputeamount=value;}
		}
		private decimal? _priceonrequest;
		/// <summary>
		/// 存在价格争议时，客户经理要求的价格
		/// </summary>
		public decimal? priceonrequest
		{
			get{ return _priceonrequest;}
			set{ _priceonrequest=value;}
		}
		private string _pricedisputeremark;
		/// <summary>
		/// 价格争议备注
		/// </summary>
		public string pricedisputeremark
		{
			get{ return _pricedisputeremark;}
			set{ _pricedisputeremark=value;}
		}
		private int? _appraisersuserid;
		/// <summary>
		/// 审核估价师
		/// </summary>
		public int? appraisersuserid
		{
			get{ return _appraisersuserid;}
			set{ _appraisersuserid=value;}
		}
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal buildingarea { get; set; }
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
        private decimal? _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private string _remark;
        /// <summary>
        /// 外部 预评备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
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
        private bool _isgetno=false;
        /// <summary>
        /// 是否领取编号
        /// </summary>
        public bool isgetno
        {
            get { return _isgetno; }
            set { _isgetno = value; }
        }
        private int? _ypwriteruserid;
        /// <summary>
        /// 预评的撰写人
        /// </summary>
        public int? ypwriteruserid
        {
            get { return _ypwriteruserid; }
            set { _ypwriteruserid = value; }
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
        private int? _reportwriteruserid;
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public int? reportwriteruserid
        {
            get { return _reportwriteruserid; }
            set { _reportwriteruserid = value; }
        }
        private bool _istoreport=false;
        /// <summary>
        /// 是否已转报告
        /// </summary>
        public bool istoreport
        {
            get {
                if (_istoreport == null)
                    _istoreport = false;
                return _istoreport; 
            }
            set { _istoreport = value; }
        }

        private int _appraisersuserid1;
        /// <summary>
        /// 审核估价师
        /// </summary>
        public int appraisersuserid1
        {
            get
            {
                return _appraisersuserid1;
            }
            set { _appraisersuserid1 = value; }
        }

        private string _appraisersusernumber1;
        /// <summary>
        /// 审核估价师注册号
        /// </summary>
        public string appraisersusernumber1
        {
            get
            {
                return _appraisersusernumber1;
            }
            set { _appraisersusernumber1 = value; }
        }

        private int _appraisersuserid2;
        /// <summary>
        /// 项目估价师
        /// </summary>
        public int appraisersuserid2
        {
            get
            {
                return _appraisersuserid2;
            }
            set { _appraisersuserid2 = value; }
        }

        private string _appraisersusernumber2;
        /// <summary>
        /// 项目估价师注册号
        /// </summary>
        public string appraisersusernumber2
        {
            get
            {
                return _appraisersusernumber2;
            }
            set { _appraisersusernumber2 = value; }
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
        private DateTime? _valuationdate;
        /// <summary>
        /// 价值时点
        /// </summary>
         public DateTime? valuationdate
        {
            get { return _valuationdate; }
            set { _valuationdate = value; }
        }
	}
}   