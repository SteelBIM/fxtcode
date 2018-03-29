using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Log4net")]
	public class Log4net : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _level;
		public string level
		{
			get{ return _level;}
			set{ _level=value;}
		}
		private string _logger;
		public string logger
		{
			get{ return _logger;}
			set{ _logger=value;}
		}
		private string _host;
		public string host
		{
			get{ return _host;}
			set{ _host=value;}
		}
		private DateTime? _date;
		public DateTime? date
		{
			get{ return _date;}
			set{ _date=value;}
		}
		private string _thread;
		public string thread
		{
			get{ return _thread;}
			set{ _thread=value;}
		}
		private string _message;
		public string message
		{
			get{ return _message;}
			set{ _message=value;}
		}
		private string _exception;
		public string exception
		{
			get{ return _exception;}
			set{ _exception=value;}
		}
	}
}