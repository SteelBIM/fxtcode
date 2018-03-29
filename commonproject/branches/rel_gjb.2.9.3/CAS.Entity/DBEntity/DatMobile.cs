using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_Mobile")]
	public class DatMobile : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _fasonguser;
		/// <summary>
		/// 发送人
		/// </summary>
		public string fasonguser
		{
			get{ return _fasonguser;}
			set{ _fasonguser=value;}
		}
		private string _touserlist;
		/// <summary>
		/// 接收人
		/// </summary>
		public string touserlist
		{
			get{ return _touserlist;}
			set{ _touserlist=value;}
		}
		private string _contentstr;
		/// <summary>
		/// 短信内容
		/// </summary>
		public string contentstr
		{
			get{ return _contentstr;}
			set{ _contentstr=value;}
		}
		private DateTime _timestr = DateTime.Now;
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
	}
}