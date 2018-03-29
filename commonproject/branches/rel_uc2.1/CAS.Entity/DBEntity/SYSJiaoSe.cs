using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_JiaoSe")]
	public class SYSJiaoSe : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
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
		private string _jiaosename;
		/// <summary>
		/// 角色名称
		/// </summary>
		public string jiaosename
		{
			get{ return _jiaosename;}
			set{ _jiaosename=value;}
		}
		private string _backinfo;
		/// <summary>
		/// 备注说明
		/// </summary>
		public string backinfo
		{
			get{ return _backinfo;}
			set{ _backinfo=value;}
		}
		private string _quanxian;
		/// <summary>
		/// 权限
		/// </summary>
		public string quanxian
		{
			get{ return _quanxian;}
			set{ _quanxian=value;}
		}
		private int _valid = 1;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}