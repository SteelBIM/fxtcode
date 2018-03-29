using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_UserDesk")]
	public class DatUserDesk : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _username;
		/// <summary>
		/// 用户名
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private string _modelname;
		/// <summary>
		/// 模块名
		/// </summary>
		public string modelname
		{
			get{ return _modelname;}
			set{ _modelname=value;}
		}
		private int _looknum = 5;
		/// <summary>
		/// 显示数量
		/// </summary>
		public int looknum
		{
			get{ return _looknum;}
			set{ _looknum=value;}
		}
	}
}