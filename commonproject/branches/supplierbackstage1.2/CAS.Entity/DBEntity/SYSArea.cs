using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_Area")]
	public class SYSArea : BaseTO
	{
		private int _areaid;
		[SQLField("areaid", EnumDBFieldUsage.PrimaryKey, true)]
		public int areaid
		{
			get{ return _areaid;}
			set{ _areaid=value;}
		}
		private string _areaname;
		/// <summary>
		/// 区域名称
		/// </summary>
		public string areaname
		{
			get{ return _areaname;}
			set{ _areaname=value;}
		}
		private int _cityid;
		public int cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private decimal? _x;
		/// <summary>
		/// 坐标X
		/// </summary>
		public decimal? x
		{
			get{ return _x;}
			set{ _x=value;}
		}
		private decimal? _y;
		/// <summary>
		/// 坐标Y
		/// </summary>
		public decimal? y
		{
			get{ return _y;}
			set{ _y=value;}
		}
		private int? _orderid;
		/// <summary>
		/// 排序
		/// </summary>
		public int? orderid
		{
			get{ return _orderid;}
			set{ _orderid=value;}
		}
	}
}