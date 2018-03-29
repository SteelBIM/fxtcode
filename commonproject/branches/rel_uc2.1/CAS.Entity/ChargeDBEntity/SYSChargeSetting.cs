using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_ChargeSetting")]
	public class SYSChargeSetting : BaseTO
	{
		private int _companyid;
		/// <summary>
		/// 公司Id
		/// </summary>
		[SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
		public int companyid
		{
			get{ return _companyid;}
			set{ _companyid=value;}
		}
		private int _chargetype;
		/// <summary>
		/// 1,国家标准2,自定义标准          
		/// </summary>
		public int chargetype
		{
			get{ return _chargetype;}
			set{ _chargetype=value;}
		}
		private DateTime _createtime = DateTime.Now;
		/// <summary>
		/// 录入时间
		/// </summary>
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
		private int _createrid;
		/// <summary>
		/// 录入人
		/// </summary>
		public int createrid
		{
			get{ return _createrid;}
			set{ _createrid=value;}
		}
	}
}