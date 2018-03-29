using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkDetails")]
	public class DatNWorkDetails : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _workid;
		/// <summary>
		/// 对应工作
		/// </summary>
		public int? workid
		{
			get{ return _workid;}
			set{ _workid=value;}
		}
		private string _itemsnamecn;
		/// <summary>
		/// 列中文名
		/// </summary>
		public string itemsnamecn
		{
			get{ return _itemsnamecn;}
			set{ _itemsnamecn=value;}
		}
		private string _itemsnameen;
		/// <summary>
		/// 列英文名
		/// </summary>
		public string itemsnameen
		{
			get{ return _itemsnameen;}
			set{ _itemsnameen=value;}
		}
		private string _itemsvalue;
		/// <summary>
		/// 列值
		/// </summary>
		public string itemsvalue
		{
			get{ return _itemsvalue;}
			set{ _itemsvalue=value;}
		}
	}
}