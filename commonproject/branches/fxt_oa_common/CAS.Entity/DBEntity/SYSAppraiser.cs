using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_Appraiser")]
	public class SYSAppraiser : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int _subcompanyid;
		/// <summary>
		/// 分支机构
		/// </summary>
		public int subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private int _typecode;
		/// <summary>
		/// 房地产/土地/资产
		/// </summary>
		public int typecode
		{
			get{ return _typecode;}
			set{ _typecode=value;}
		}
		private string _appraisersids;
		/// <summary>
		/// 估价师
		/// </summary>
		public string appraisersids
		{
			get{ return _appraisersids;}
			set{ _appraisersids=value;}
		}
		private string _appraisernames;
		/// <summary>
		/// 估价师名称
		/// </summary>
		public string appraisernames
		{
			get{ return _appraisernames;}
			set{ _appraisernames=value;}
		}
		private int _createuserid;
		/// <summary>
		/// 创建人
		/// </summary>
		public int createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}
		private DateTime _createtime = DateTime.Now;
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
	}
}