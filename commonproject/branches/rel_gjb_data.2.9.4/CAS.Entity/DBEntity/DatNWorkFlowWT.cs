using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkFlowWT")]
	public class DatNWorkFlowWT : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _fromuser;
		/// <summary>
		/// 委托人
		/// </summary>
		public string fromuser
		{
			get{ return _fromuser;}
			set{ _fromuser=value;}
		}
		private string _touser;
		/// <summary>
		/// 被委托人
		/// </summary>
		public string touser
		{
			get{ return _touser;}
			set{ _touser=value;}
		}
        private DateTime? _days;
        /// <summary>
        /// 有效时间始
        /// </summary>
        public DateTime? days
		{
			get{ return _days;}
			set{ _days=value;}
		}
		private DateTime? _daytime;
        /// <summary>
        /// 有效时间结束
        /// </summary>
		public DateTime? daytime
		{
			get{ return _daytime;}
			set{ _daytime=value;}
		}
	}
}