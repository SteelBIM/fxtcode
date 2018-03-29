using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    /// <summary>
    /// 产品对应功能配置表
    /// </summary>
	[Serializable]
    [TableAttribute("dbo.ProductSetting")]
    public class ProductSetting : BaseTO
	{
		private int _producttypecode;
		/// <summary>
		/// 产品CODE
		/// </summary>
		[SQLField("producttypecode", EnumDBFieldUsage.PrimaryKey)]
		public int producttypecode
		{
			get{ return _producttypecode;}
			set{ _producttypecode=value;}
		}
        private int _actionid;
		/// <summary>
		/// 功能编号
		/// </summary>
        [SQLField("actionid", EnumDBFieldUsage.PrimaryKey)]
        public int actionid
		{
            get { return _actionid; }
            set { _actionid = value; }
		}
		private int _isopen;
		/// <summary>
		/// 是否开通功能
		/// </summary>
		public int isopen
		{
			get{ return _isopen;}
			set{ _isopen=value;}
		}
		private string _apiurl;
		/// <summary>
		/// Api URL地址
		/// </summary>
		public string apiurl
		{
			get{ return _apiurl;}
			set{ _apiurl=value;}
		}
		private string _apiname;
		/// <summary>
		/// Api名称
		/// </summary>
		public string apiname
		{
			get{ return _apiname;}
			set{ _apiname=value;}
		}
		private string _callapiurl;
		/// <summary>
		/// 回调Api URL地址
		/// </summary>
		public string callapiurl
		{
			get{ return _callapiurl;}
			set{ _callapiurl=value;}
		}
		private string _callapiname;
		/// <summary>
		/// 回调Api方法
		/// </summary>
		public string callapiname
		{
			get{ return _callapiname;}
			set{ _callapiname=value;}
		}
		private string _remark;
		/// <summary>
		/// 备注
		/// </summary>
		public string remark
		{
			get{ return _remark;}
			set{ _remark=value;}
		}
	}
}