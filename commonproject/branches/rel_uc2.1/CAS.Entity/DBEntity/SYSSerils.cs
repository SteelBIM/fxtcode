using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_Serils")]
	public class SYSSerils : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _serilsstr;
		public string serilsstr
		{
			get{ return _serilsstr;}
			set{ _serilsstr=value;}
		}
		private string _datestr;
		public string datestr
		{
			get{ return _datestr;}
			set{ _datestr=value;}
		}
		private string _usernum;
		public string usernum
		{
			get{ return _usernum;}
			set{ _usernum=value;}
		}
		private string _danweistr;
		public string danweistr
		{
			get{ return _danweistr;}
			set{ _danweistr=value;}
		}
	}
}