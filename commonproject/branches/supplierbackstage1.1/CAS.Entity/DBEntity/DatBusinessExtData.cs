using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_BusinessExtData")]
	public class DatBusinessExtData : BaseTO
	{
		private long _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public long id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private long _objectid;
		/// <summary>
		/// 业务ID
		/// </summary>
		public long objectid
		{
			get{ return _objectid;}
			set{ _objectid=value;}
		}
        public int btsid { get; set; }
		private string _formdata;
		/// <summary>
		/// 业务数据
		/// </summary>
		public string formdata
		{
			get{ return _formdata;}
			set{ _formdata=value;}
		}
	}
}