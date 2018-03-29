using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_TaxTemplate")]
	public class SYSTaxTemplate : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int _cityid;
		public int cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private int _companyid;
		/// <summary>
		/// 公司ID
		/// </summary>
		public int companyid
		{
			get{ return _companyid;}
			set{ _companyid=value;}
		}
		private string _templatename;
		/// <summary>
		/// 模板名称
		/// </summary>
		public string templatename
		{
			get{ return _templatename;}
			set{ _templatename=value;}
		}
		private int _propertytype;
		/// <summary>
		/// 物业类型, 普通住宅-1，非普通住宅-2，非住宅-3
		/// </summary>
		public int propertytype
		{
			get{ return _propertytype;}
			set{ _propertytype=value;}
		}
		private int _ownertype;
		/// <summary>
		/// 权属类型，个人-1，企业-2，忽略-3
		/// </summary>
		public int ownertype
		{
			get{ return _ownertype;}
			set{ _ownertype=value;}
		}
		private int _isover5year;
		/// <summary>
		/// 是否满五年，未满-1，已满-2，忽略-3
		/// </summary>
		public int isover5year
		{
			get{ return _isover5year;}
			set{ _isover5year=value;}
		}
		private int _isfirstbuy;
		/// <summary>
		/// 是否首次购房
		/// </summary>
		public int isfirstbuy
		{
			get{ return _isfirstbuy;}
			set{ _isfirstbuy=value;}
		}
		private int _isuniquehouse;
		/// <summary>
		/// 是否家庭唯一生活用房
		/// </summary>
		public int isuniquehouse
		{
			get{ return _isuniquehouse;}
			set{ _isuniquehouse=value;}
		}
		private int _areatypecode;
		/// <summary>
		/// 面积分段（60,90,144）
		/// </summary>
		public int areatypecode
		{
			get{ return _areatypecode;}
			set{ _areatypecode=value;}
		}
		private DateTime _templatedate;
		/// <summary>
		/// 税费公式生效日期
		/// </summary>
		public DateTime templatedate
		{
			get{ return _templatedate;}
			set{ _templatedate=value;}
		}
		private int _bankid = 0;
		/// <summary>
		/// 银行ID,fxtproject.dbo.privi_company外键
		/// </summary>
		public int bankid
		{
			get{ return _bankid;}
			set{ _bankid=value;}
		}
		private string _d;
		/// <summary>
		/// 营业税公式。A原购价，B评估总值，C建筑面积
		/// </summary>
		public string d
		{
			get{ return _d;}
			set{ _d=value;}
		}
		private string _e;
		/// <summary>
		/// 城建税公式
		/// </summary>
		public string e
		{
			get{ return _e;}
			set{ _e=value;}
		}
		private string _f;
		/// <summary>
		/// 教育附加税公式
		/// </summary>
		public string f
		{
			get{ return _f;}
			set{ _f=value;}
		}
		private string _g;
		/// <summary>
		/// 印花税公式
		/// </summary>
		public string g
		{
			get{ return _g;}
			set{ _g=value;}
		}
		private string _h;
		/// <summary>
		/// 契税公式
		/// </summary>
		public string h
		{
			get{ return _h;}
			set{ _h=value;}
		}
		private string _i;
		/// <summary>
		/// 处置费用公式
		/// </summary>
		public string i
		{
			get{ return _i;}
			set{ _i=value;}
		}
		private string _j;
		/// <summary>
		/// 交易手续费公式
		/// </summary>
		public string j
		{
			get{ return _j;}
			set{ _j=value;}
		}
		private string _k;
		/// <summary>
		/// 土地增值税公式
		/// </summary>
		public string k
		{
			get{ return _k;}
			set{ _k=value;}
		}
		private string _l;
		/// <summary>
		/// 所得税公式
		/// </summary>
		public string l
		{
			get{ return _l;}
			set{ _l=value;}
		}
		private string _totaltax;
		/// <summary>
		/// 税费总额公式
		/// </summary>
		public string totaltax
		{
			get{ return _totaltax;}
			set{ _totaltax=value;}
		}
		private DateTime _createtime = DateTime.Now;
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
		private string _createuser;
		public string createuser
		{
			get{ return _createuser;}
			set{ _createuser=value;}
		}
		private DateTime _savetime = DateTime.Now;
		public DateTime savetime
		{
			get{ return _savetime;}
			set{ _savetime=value;}
		}
		private int? _saveuserid;
		public int? saveuserid
		{
			get{ return _saveuserid;}
			set{ _saveuserid=value;}
		}
		private int _valid = 1;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}