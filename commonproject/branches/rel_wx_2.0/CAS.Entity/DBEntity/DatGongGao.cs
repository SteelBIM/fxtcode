using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_GongGao")]
	public class DatGongGao : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _titlestr;
		/// <summary>
		/// 公告主题
		/// </summary>
		public string titlestr
		{
			get{ return _titlestr;}
			set{ _titlestr=value;}
		}
		private DateTime _timestr = DateTime.Now;
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
		private string _username;
		/// <summary>
		/// 用户名
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private string _userbumen;
		/// <summary>
		/// 接收部门
		/// </summary>
		public string userbumen
		{
			get{ return _userbumen;}
			set{ _userbumen=value;}
		}
		private string _fujian;
		/// <summary>
		/// 附件文件
		/// </summary>
		public string fujian
		{
			get{ return _fujian;}
			set{ _fujian=value;}
		}
		private string _contentstr;
		/// <summary>
		/// 详细内容
		/// </summary>
		public string contentstr
		{
			get{ return _contentstr;}
			set{ _contentstr=value;}
		}
		private string _typestr;
		/// <summary>
		/// 分类
		/// </summary>
		public string typestr
		{
			get{ return _typestr;}
			set{ _typestr=value;}
		}
        private string _typename;
        /// <summary>
        /// 公告分类名称
        /// </summary>
        public string typename
        {
            get { return _typename; }
            set { _typename = value; }
        }

	}
}