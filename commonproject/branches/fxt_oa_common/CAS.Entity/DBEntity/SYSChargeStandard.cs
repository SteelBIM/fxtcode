using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_ChargeStandard")]
	public class SYSChargeStandard : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int _fxtcompanyid = 0;
		/// <summary>
		/// 公司ID
		/// </summary>
		public int fxtcompanyid
		{
			get{ return _fxtcompanyid;}
			set{ _fxtcompanyid=value;}
		}
		private int? _subcompanyid;
		/// <summary>
		/// 分支机构Id
		/// </summary>
		public int? subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private int _ownertype = 0;
		/// <summary>
		/// 权属：个人，企业
		/// </summary>
		public int ownertype
		{
			get{ return _ownertype;}
			set{ _ownertype=value;}
		}
		private string _chargetype;
		/// <summary>
		/// 收费类型
		/// </summary>
		public string chargetype
		{
			get{ return _chargetype;}
			set{ _chargetype=value;}
		}
		private int _pricelow = 0;
		/// <summary>
		/// 价格区间的低价
		/// </summary>
		public int pricelow
		{
			get{ return _pricelow;}
			set{ _pricelow=value;}
		}
		private int _pricehigh = 0;
		/// <summary>
		/// 价格区间的高价
		/// </summary>
		public int pricehigh
		{
			get{ return _pricehigh;}
			set{ _pricehigh=value;}
		}
		private double _nationalstandard = 0;
		/// <summary>
		/// 国家标准
		/// </summary>
		public double nationalstandard
		{
			get{ return _nationalstandard;}
			set{ _nationalstandard=value;}
		}
		private double _companystandard = 0;
		/// <summary>
		/// 公司标准
		/// </summary>
		public double companystandard
		{
			get{ return _companystandard;}
			set{ _companystandard=value;}
		}
		private double _lowestprice = 0;
		/// <summary>
		/// 最低价格
		/// </summary>
		public double lowestprice
		{
			get{ return _lowestprice;}
			set{ _lowestprice=value;}
		}
		private int _createuserid = 0;
		public int createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}
		private DateTime _createtime = DateTime.Now;
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
	}
}