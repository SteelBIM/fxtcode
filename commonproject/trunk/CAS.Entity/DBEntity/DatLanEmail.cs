using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_LanEmail")]
	public class DatLanEmail : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
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
		private string _emailcontent;
		/// <summary>
		/// 邮件内容
		/// </summary>
		public string emailcontent
		{
			get{ return _emailcontent;}
			set{ _emailcontent=value;}
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
		private string _fromuser;
		/// <summary>
		/// 发送人
		/// </summary>
		public string fromuser
		{
			get{ return _fromuser;}
			set{ _fromuser=value;}
		}
		private string _touser;
		/// <summary>
		/// 接收人
		/// </summary>
		public string touser
		{
			get{ return _touser;}
			set{ _touser=value;}
		}
		private string _emailstate = "未读";
		/// <summary>
		/// 邮件状态
		/// </summary>
		public string emailstate
		{
			get{ return _emailstate;}
			set{ _emailstate=value;}
		}
		private string _ifdel;
		/// <summary>
		/// 发件人是否删除
		/// </summary>
		public string ifdel
		{
			get{ return _ifdel;}
			set{ _ifdel=value;}
		}
		private int? _fromuserid;
		/// <summary>
		/// 发送人ID
		/// </summary>
		public int? fromuserid
		{
			get{ return _fromuserid;}
			set{ _fromuserid=value;}
		}
		private int? _touserid;
		/// <summary>
		/// 接收人ID
		/// </summary>
		public int? touserid
		{
			get{ return _touserid;}
			set{ _touserid=value;}
		}
		private bool _isread = false;
		/// <summary>
		/// 是否阅读
		/// </summary>
		public bool isread
		{
			get{ return _isread;}
			set{ _isread=value;}
		}
		private bool _valid = true;
		/// <summary>
		/// 有效
		/// </summary>
		public bool valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
        private bool _shouvalid = true;
        /// <summary>
        /// 发送箱是否有效
        /// </summary>
        public bool shouvalid
        {
            get { return _shouvalid; }
            set { _shouvalid = value; }
        }
        private int _type = 1;
        /// <summary>
        /// 消息类型，1：用户之间发送的消息。2：任务消息。
        /// </summary>
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _fromusername;
        /// <summary>
        /// 邮件发送人
        /// </summary>
        public string fromusername
        {
            get { return _fromusername; }
            set { _fromusername = value; }
        }
        private string _tousername;
        /// <summary>
        /// 邮件接收人
        /// </summary>
        public string tousername
        {
            get { return _tousername; }
            set { _tousername = value; }
        }
        private string _emailtitle;
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string emailtitle
        {
            get { return _emailtitle; }
            set { _emailtitle = value; }
        }
        private string _source = string.Empty;
        /// <summary>
        /// 消息来源 1、cas:来源CAS端 2、wx:来源微信端
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private int? _businesstype;
        /// <summary>
        /// 业务类型 1：询价、2：预评、3：报告、4：业务、5：行政审批、6：查勘、7：待归档、8：文件传阅
        /// </summary>
        public int? businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private long? _businessid;
        /// <summary>
        /// 业务ID
        /// </summary>
        public long? businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
	}
}