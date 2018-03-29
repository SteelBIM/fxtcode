using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkBusinessErrorTypes")]
	public class DatNWorkBusinessErrorTypes : BaseTO
	{
		private int _typeid;
		[SQLField("typeid", EnumDBFieldUsage.PrimaryKey, true)]
		public int typeid
		{
			get{ return _typeid;}
			set{ _typeid=value;}
		}
		private string _typename;
		public string typename
		{
			get{ return _typename;}
			set{ _typename=value;}
		}
		private DateTime _createdon = DateTime.Now;
		public DateTime createdon
		{
			get{ return _createdon;}
			set{ _createdon=value;}
		}
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}