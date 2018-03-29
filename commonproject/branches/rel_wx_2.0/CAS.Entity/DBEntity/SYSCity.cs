using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_City")]
	public class SYSCity : BaseTO
	{
		private int _cityid;
		[SQLField("cityid", EnumDBFieldUsage.PrimaryKey, true)]
		public int cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private string _cityname;
		/// <summary>
		/// 城市名称
		/// </summary>
		public string cityname
		{
			get{ return _cityname;}
			set{ _cityname=value;}
		}
		private string _alias;
		/// <summary>
		/// 城市别名
		/// </summary>
		public string alias
		{
			get{ return _alias;}
			set{ _alias=value;}
		}
		private int _provinceid;
		public int provinceid
		{
			get{ return _provinceid;}
			set{ _provinceid=value;}
		}
		private string _citycode;
		/// <summary>
		/// 区号
		/// </summary>
		public string citycode
		{
			get{ return _citycode;}
			set{ _citycode=value;}
		}
		private decimal? _x;
		public decimal? x
		{
			get{ return _x;}
			set{ _x=value;}
		}
		private decimal? _y;
		public decimal? y
		{
			get{ return _y;}
			set{ _y=value;}
		}
	}
}