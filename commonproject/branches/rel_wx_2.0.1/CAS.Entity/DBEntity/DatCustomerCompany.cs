using System;
using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_CustomerCompany")]
	public class DatCustomerCompany : BaseTO
	{
		private int _customercompanyid;
		[SQLField("customercompanyid", EnumDBFieldUsage.PrimaryKey, true)]
		public int customercompanyid
		{
			get{ return _customercompanyid;}
			set{ _customercompanyid=value;}
		}
		private int _danweiid;
		public int danweiid
		{
			get{ return _danweiid;}
			set{ _danweiid=value;}
		}
		private int? _cityid;
		public int? cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private string _codename;
		/// <summary>
		/// 客户公司类型
		/// </summary>
		public string codename
		{
			get{ return _codename;}
			set{ _codename=value;}
		}
		private string _customercompanyname;
		public string customercompanyname
		{
			get{ return _customercompanyname;}
			set{ _customercompanyname=value;}
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
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}

        private string _companypy;
        /// <summary>
        /// 客户名称拼音首字母
        /// </summary>
        public string companypy
        {
            get { return _companypy; }
            set { _companypy = value; }
        }
	}
}