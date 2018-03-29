using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NForm")]
	public class DatNForm : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _formname;
		/// <summary>
		/// 表单名称
		/// </summary>
		public string formname
		{
			get{ return _formname;}
			set{ _formname=value;}
		}
		private int? _typeid;
		/// <summary>
		/// 所属分类ID
		/// </summary>
		public int? typeid
		{
			get{ return _typeid;}
			set{ _typeid=value;}
		}
		private string _userlistok;
		/// <summary>
		/// 允许使用人
		/// </summary>
		public string userlistok
		{
			get{ return _userlistok;}
			set{ _userlistok=value;}
		}
		private string _deplistok;
		/// <summary>
		/// 允许使用部门
		/// </summary>
		public string deplistok
		{
			get{ return _deplistok;}
			set{ _deplistok=value;}
		}
		private string _jiaoselistok;
		/// <summary>
		/// 允许使用角色
		/// </summary>
		public string jiaoselistok
		{
			get{ return _jiaoselistok;}
			set{ _jiaoselistok=value;}
		}
		private string _paixustr;
		/// <summary>
		/// 排序字符
		/// </summary>
		public string paixustr
		{
			get{ return _paixustr;}
			set{ _paixustr=value;}
		}
		private string _username;
		/// <summary>
		/// 录入人
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private DateTime? _timestr;
		/// <summary>
		/// 录入时间
		/// </summary>
		public DateTime? timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
		private string _contentstr;
		/// <summary>
		/// 表单内容
		/// </summary>
		public string contentstr
		{
			get{ return _contentstr;}
			set{ _contentstr=value;}
		}
		private string _itemslist;
		/// <summary>
		/// 表单中数据列
		/// </summary>
		public string itemslist
		{
			get{ return _itemslist;}
			set{ _itemslist=value;}
		}
		private string _ifok;
		/// <summary>
		/// 是否启用
		/// </summary>
		public string ifok
		{
			get{ return _ifok;}
			set{ _ifok=value;}
		}
		private string _formdataname;
		public string formdataname
		{
			get{ return _formdataname;}
			set{ _formdataname=value;}
		}
		private string _itemlist;
		public string itemlist
		{
			get{ return _itemlist;}
			set{ _itemlist=value;}
		}
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
		private int _parentid = 0;
		/// <summary>
		/// 为空用0表示
		/// </summary>
		public int parentid
		{
			get{ return _parentid;}
			set{ _parentid=value;}
		}
	}
}