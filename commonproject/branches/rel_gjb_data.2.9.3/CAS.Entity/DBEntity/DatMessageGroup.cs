using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_Message_Group")]
	public class DatMessageGroup : BaseTO
	{
		private int _groupid;
		/// <summary>
		/// 群组ID
		/// </summary>
		[SQLField("groupid", EnumDBFieldUsage.PrimaryKey, true)]
		public int groupid
		{
			get{ return _groupid;}
			set{ _groupid=value;}
		}
		private string _groupname;
		/// <summary>
		/// 群组名称
		/// </summary>
		public string groupname
		{
			get{ return _groupname;}
			set{ _groupname=value;}
		}
		private string _groupdesc;
		/// <summary>
		/// 说明
		/// </summary>
		public string groupdesc
		{
			get{ return _groupdesc;}
			set{ _groupdesc=value;}
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
		private DateTime _createdate = DateTime.Now;
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime createdate
		{
			get{ return _createdate;}
			set{ _createdate=value;}
		}
        private string _users;
        /// <summary>
        /// 说明
        /// </summary>
        public string users
        {
            get { return _users; }
            set { _users = value; }
        }
	}
}