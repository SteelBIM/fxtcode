using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_ShenPi")]
	public class DatShenPi : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _contentstr;
		public string contentstr
		{
			get{ return _contentstr;}
			set{ _contentstr=value;}
		}
		private DateTime? _timestr;
		public DateTime? timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
		private string _username;
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
	}
}