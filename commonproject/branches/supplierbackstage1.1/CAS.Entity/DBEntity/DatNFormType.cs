using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NFormType")]
	public class DatNFormType : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _typename;
		/// <summary>
		/// 分类名称
		/// </summary>
		public string typename
		{
			get{ return _typename;}
			set{ _typename=value;}
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
		private string _backinfo;
		/// <summary>
		/// 简要说明
		/// </summary>
		public string backinfo
		{
			get{ return _backinfo;}
			set{ _backinfo=value;}
		}
		private bool _canedit = true;
		public bool canedit
		{
			get{ return _canedit;}
			set{ _canedit=value;}
		}
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}