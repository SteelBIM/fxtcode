using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_SaveFileName")]
	public class DatSaveFileName : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _nowname;
		/// <summary>
		/// 新文件名
		/// </summary>
		public string nowname
		{
			get{ return _nowname;}
			set{ _nowname=value;}
		}
		private string _oldname;
		/// <summary>
		/// 原文件名
		/// </summary>
		public string oldname
		{
			get{ return _oldname;}
			set{ _oldname=value;}
		}
	}
}