using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Article")]
	public class Article : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _title;
		public string title
		{
			get{ return _title;}
			set{ _title=value;}
		}
		private string _content;
		public string content
		{
			get{ return _content;}
			set{ _content=value;}
		}
		private int _channelid;
		public int channelid
		{
			get{ return _channelid;}
			set{ _channelid=value;}
		}
		private string _coverpicture;
		public string coverpicture
		{
			get{ return _coverpicture;}
			set{ _coverpicture=value;}
		}
		private int _hits = 0;
		public int hits
		{
			get{ return _hits;}
			set{ _hits=value;}
		}
		private int _diggs = 0;
		public int diggs
		{
			get{ return _diggs;}
			set{ _diggs=value;}
		}
		private bool _isactive = false;
		public bool isactive
		{
			get{ return _isactive;}
			set{ _isactive=value;}
		}
		private int _userid;
		public int userid
		{
			get{ return _userid;}
			set{ _userid=value;}
		}
		private string _username = 0;
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private DateTime _createtime = DateTime.Now;
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
	}
}