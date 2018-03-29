using System;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
	[Serializable]
	[TableAttribute("dbo.Dat_PushMessage")]
	public class DatPushMessage : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _pushid;
		/// <summary>
		/// Dat_MobilePush  表 Id
		/// </summary>
		public int? pushid
		{
			get{ return _pushid;}
			set{ _pushid=value;}
		}
		private DateTime _createdate = DateTime.Now;
		public DateTime createdate
		{
			get{ return _createdate;}
			set{ _createdate=value;}
		}
		private string _neirong;
		public string neirong
		{
			get{ return _neirong;}
			set{ _neirong=value;}
		}
		private string _title;
		public string title
		{
			get{ return _title;}
			set{ _title=value;}
		}
		private string _result;
		public string result
		{
			get{ return _result;}
			set{ _result=value;}
		}
	}
}