using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_CustomerBranchCompany")]
	public class DatCustomerBranchCompany : BaseTO
	{
		private int _branchcompanyid;
		[SQLField("branchcompanyid", EnumDBFieldUsage.PrimaryKey, true)]
		public int branchcompanyid
		{
			get{ return _branchcompanyid;}
			set{ _branchcompanyid=value;}
		}
		private int _fk_customercompanyid;
		/// <summary>
		/// 客户总公司ID
		/// </summary>
		public int fk_customercompanyid
		{
			get{ return _fk_customercompanyid;}
			set{ _fk_customercompanyid=value;}
		}
		private string _branchname;
		/// <summary>
		/// 机构名称
		/// </summary>
		public string branchname
		{
			get{ return _branchname;}
			set{ _branchname=value;}
		}
		private int _branchcompanyattr;
		/// <summary>
		/// 1是分支机构 2是子机构
		/// </summary>
		public int branchcompanyattr
		{
			get{ return _branchcompanyattr;}
			set{ _branchcompanyattr=value;}
		}
		private int? _cityid;
		/// <summary>
		/// 城市ID(为分支机构时,CityId不允许为空)
		/// </summary>
		public int? cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private int _parentid = 0;
		/// <summary>
		/// 父ID
		/// </summary>
		public int parentid
		{
			get{ return _parentid;}
			set{ _parentid=value;}
		}
		private string _officephone;
		/// <summary>
		/// 办公电话
		/// </summary>
		public string officephone
		{
			get{ return _officephone;}
			set{ _officephone=value;}
		}
		private string _address;
		/// <summary>
		/// 地址
		/// </summary>
		public string address
		{
			get{ return _address;}
			set{ _address=value;}
		}
		private string _email;
		/// <summary>
		/// Email
		/// </summary>
		public string email
		{
			get{ return _email;}
			set{ _email=value;}
		}
		private string _fax;
		/// <summary>
		/// 传真
		/// </summary>
		public string fax
		{
			get{ return _fax;}
			set{ _fax=value;}
		}
		private string _contact;
		/// <summary>
		/// 联系人
		/// </summary>
		public string contact
		{
			get{ return _contact;}
			set{ _contact=value;}
		}
		private DateTime _createdon;
		public DateTime createdon
		{
			get{ return _createdon;}
			set{ _createdon=value;}
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
	}
}