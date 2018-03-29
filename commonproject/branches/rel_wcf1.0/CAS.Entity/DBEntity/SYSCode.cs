using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_Code")]
	public class SYSCode : BaseTO
	{
		private int _codeid;
		[SQLField("codeid", EnumDBFieldUsage.PrimaryKey, true)]
		public int codeid
		{
			get{ return _codeid;}
			set{ _codeid=value;}
		}
		private int _id;
		/// <summary>
		/// 字典ID
		/// </summary>
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _code;
		/// <summary>
		/// 系统字典子项ID
		/// </summary>
		public int? code
		{
			get{ return _code;}
			set{ _code=value;}
		}
		private string _codename;
		/// <summary>
		/// 字典值
		/// </summary>
		public string codename
		{
			get{ return _codename;}
			set{ _codename=value;}
		}
		private string _codetype;
		/// <summary>
		/// 字典名称
		/// </summary>
		public string codetype
		{
			get{ return _codetype;}
			set{ _codetype=value;}
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
		private int? _subcode;
		/// <summary>
		/// 子字典
		/// </summary>
		public int? subcode
		{
			get{ return _subcode;}
			set{ _subcode=value;}
		}
		private bool _canedit = true;
		/// <summary>
		/// 可编辑（用户自定义）
		/// </summary>
		public bool canedit
		{
			get{ return _canedit;}
			set{ _canedit=value;}
		}
		private int? _orderid;
		/// <summary>
		/// 排序显示
		/// </summary>
		public int? orderid
		{
			get{ return _orderid;}
			set{ _orderid=value;}
		}
		private int? _dictype;
		/// <summary>
		/// 字典大类型
		/// </summary>
		public int? dictype
		{
			get{ return _dictype;}
			set{ _dictype=value;}
		}

        private int? _subtype;
        /// <summary>
		/// dictype的子项
		/// </summary>
        public int? subtype
		{
            get { return _subtype; }
            set { _subtype = value; }
		}
        
	}
}