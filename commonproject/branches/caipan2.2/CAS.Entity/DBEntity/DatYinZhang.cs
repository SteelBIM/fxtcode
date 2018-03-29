using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_YinZhang")]
	public class DatYinZhang : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _yinzhangname;
		/// <summary>
		/// 印章名称
		/// </summary>
		public string yinzhangname
		{
			get{ return _yinzhangname;}
			set{ _yinzhangname=value;}
		}
		private string _yinzhangleibie;
		/// <summary>
		/// 印章类别
		/// </summary>
		public string yinzhangleibie
		{
			get{ return _yinzhangleibie;}
			set{ _yinzhangleibie=value;}
		}
		private string _yinzhangmima;
		/// <summary>
		/// 印章密码
		/// </summary>
		public string yinzhangmima
		{
			get{ return _yinzhangmima;}
			set{ _yinzhangmima=value;}
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
		private string _imgpath;
		/// <summary>
		/// 印章图片
		/// </summary>
		public string imgpath
		{
			get{ return _imgpath;}
			set{ _imgpath=value;}
		}
		private DateTime _timestr = DateTime.Now;
		/// <summary>
		/// 录入时间
		/// </summary>
		public DateTime timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
	}
}