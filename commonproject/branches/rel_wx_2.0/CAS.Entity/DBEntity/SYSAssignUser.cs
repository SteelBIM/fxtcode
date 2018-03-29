using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_AssignUser")]
	public class SYSAssignUser : BaseTO
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
		private int _businesstype;
		/// <summary>
		/// 业务类型
		/// </summary>
		public int businesstype
		{
			get{ return _businesstype;}
			set{ _businesstype=value;}
		}
		private int _entrusttypecode;
		/// <summary>
		/// 对公/个人
		/// </summary>
		public int entrusttypecode
		{
			get{ return _entrusttypecode;}
			set{ _entrusttypecode=value;}
		}
        private string _assinguserids;
		/// <summary>
		/// 分配人
		/// </summary>
		public string assinguserids
		{
			get{ return _assinguserids;}
			set{ _assinguserids=value;}
		}
		private string _assingusernames;
		/// <summary>
		/// 分配人名称
		/// </summary>
		public string assingusernames
		{
			get{ return _assingusernames;}
			set{ _assingusernames=value;}
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