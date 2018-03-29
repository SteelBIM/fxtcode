using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_BusinessType")]
	public class SYSBusinessType : BaseTO
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
		private string _name;
		/// <summary>
		/// 业务类型名称
		/// </summary>
		public string name
		{
			get{ return _name;}
			set{ _name=value;}
		}
		private int _typecode;
		/// <summary>
		/// 对公/个人(200104)
		/// </summary>
		public int typecode
		{
			get{ return _typecode;}
			set{ _typecode=value;}
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
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

	}
}